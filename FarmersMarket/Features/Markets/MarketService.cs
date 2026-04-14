using FarmersMarket.Models;
using Microsoft.EntityFrameworkCore;
using static FarmersMarket.Features.Markets.MarketsDTO;
using static FarmersMarket.Features.Sellers.SellerEnums;
using static FarmersMarket.Features.Sellers.SellersDTOs;

namespace FarmersMarket.Features.Markets
{
    public class MarketService(farmersmarketContext db) : IMarketService
    {
        public async Task<PagedResult<MarketDto>> GetAllAsync(MarketQueryParams q)
        {
            var query = db.Markets
                .Include(m => m.Schedules)
                .Include(m => m.MarketSellers).ThenInclude(ms => ms.Seller)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(q.Name))
                query = query.Where(m => m.Name.Contains(q.Name));

            if (q.MarketType.HasValue)
                query = query.Where(m => m.MarketType == q.MarketType.Value);

            if (q.Day.HasValue)
                query = query.Where(m => m.Schedules.Any(s => s.Day == q.Day.Value));

            if (q.IsActive.HasValue)
                query = query.Where(m => m.IsActive == q.IsActive.Value);

            var total = await query.CountAsync();
            var items = await query
                .OrderBy(m => m.Name)
                .Skip((q.Page - 1) * q.PageSize)
                .Take(q.PageSize)
                .Select(m => ToDto(m))
                .ToListAsync();

            return new PagedResult<MarketDto>(items, total, q.Page, q.PageSize);
        }

        public async Task<MarketDto?> GetByIdAsync(int id)
        {
            var market = await db.Markets
                .Include(m => m.Schedules)
                .Include(m => m.MarketSellers).ThenInclude(ms => ms.Seller)
                .FirstOrDefaultAsync(m => m.Id == id);

            return market is null ? null : ToDto(market);
        }

        public async Task<(bool Success, int? Id, string? Error)> CreateAsync(CreateMarketRequest req)
        {
            var market = new market
            {
                Name = req.Name,
                MarketType = req.MarketType,
                Address = req.Address,
                Latitude = req.Latitude,
                Longitude = req.Longitude,
                TotalSpots = req.TotalSpots,
                OpenTime = req.OpenTime,
                CloseTime = req.CloseTime,
                Notes = req.Notes,
                Schedules = req.OperatingDays
                    .Select(d => new market_schedule { Day = d })
                    .ToList()
            };

            db.Markets.Add(market);
            await db.SaveChangesAsync();
            return (true, market.Id, null);
        }

        public async Task<(bool Success, string? Error)> UpdateAsync(int id, UpdateMarketRequest req)
        {
            var market = await db.Markets.FindAsync(id);
            if (market is null) return (false, "Η αγορά δεν βρέθηκε.");

            market.Name = req.Name;
            market.MarketType = req.MarketType;
            market.Address = req.Address;
            market.Latitude = req.Latitude;
            market.Longitude = req.Longitude;
            market.TotalSpots = req.TotalSpots;
            market.OpenTime = req.OpenTime;
            market.CloseTime = req.CloseTime;
            market.Notes = req.Notes;
            market.IsActive = req.IsActive;
            market.UpdatedAt = DateTime.UtcNow;

            await db.SaveChangesAsync();
            return (true, null);
        }

        public async Task<(bool Success, string? Error)> DeleteAsync(int id)
        {
            var market = await db.Markets.FindAsync(id);
            if (market is null) return (false, "Η αγορά δεν βρέθηκε.");

            db.Markets.Remove(market);
            await db.SaveChangesAsync();
            return (true, null);
        }

        public async Task<(bool Success, string? Error)> AssignSellerAsync(int marketId, AssignSellerRequest req)
        {
            // 1. Φέρνουμε την αγορά για να δούμε αν υπάρχει και ποιο είναι το όριο θέσεων (TotalSpots)
            var market = await db.Markets
                .Include(m => m.MarketSellers)
                .FirstOrDefaultAsync(m => m.Id == marketId);

            if (market == null)
                return (false, "Η αγορά δεν βρέθηκε.");

            // 2. Έλεγχος αν ο πωλητής υπάρχει και είναι ενεργός
            var seller = await db.Sellers
                .Include(s => s.Licenses)
                .FirstOrDefaultAsync(s => s.Id == req.SellerId);

            if (seller == null)
                return (false, "Ο πωλητής δεν βρέθηκε.");

            if (!seller.IsActive)
                return (false, "Ο πωλητής είναι απενεργοποιημένος.");

            // 3. Έλεγχος αν ο πωλητής έχει τουλάχιστον μία ενεργή άδεια
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            if (!seller.Licenses.Any(l => l.Status == LicenseStatus.Active && l.ExpiresAt > today))
            {
                return (false, "Ο πωλητής δεν διαθέτει έγκυρη/ενεργή άδεια.");
            }

            // 4. Έλεγχος αν ο πωλητής είναι ήδη στην αγορά
            if (market.MarketSellers.Any(ms => ms.SellerId == req.SellerId))
                return (false, "Ο πωλητής είναι ήδη ανατεθειμένος σε αυτή την αγορά.");

            // 5. Έλεγχος αν η συγκεκριμένη θέση (SpotNumber) είναι ήδη κατειλημμένη
            if (market.MarketSellers.Any(ms => ms.SpotNumber == req.SpotNumber && ms.IsActive))
                return (false, $"Η θέση {req.SpotNumber} είναι ήδη κατειλημμένη από άλλον πωλητή.");

            // 6. Έλεγχος χωρητικότητας (Capacity Check)
            var activeSellersCount = market.MarketSellers.Count(ms => ms.IsActive);
            if (activeSellersCount >= market.TotalSpots)
                return (false, "Η αγορά είναι πλήρης (δεν υπάρχουν διαθέσιμες θέσεις).");

            // Αν όλα είναι OK, προχωράμε στην προσθήκη
            db.MarketSellers.Add(new market_seller
            {
                MarketId = marketId,
                SellerId = req.SellerId,
                SpotNumber = req.SpotNumber,
                SpotLength = req.SpotLength,
                AssignedAt = DateTime.UtcNow,
                IsActive = true
            });

            await db.SaveChangesAsync();
            return (true, null);
        }

        public async Task<(bool Success, string? Error)> RemoveSellerAsync(int marketId, int sellerId)
        {
            var ms = await db.MarketSellers
                .FirstOrDefaultAsync(x => x.MarketId == marketId && x.SellerId == sellerId);

            if (ms is null) return (false, "Η ανάθεση δεν βρέθηκε.");

            db.MarketSellers.Remove(ms);
            await db.SaveChangesAsync();
            return (true, null);
        }

        public async Task<(bool Success, string? Error)> AddScheduleExceptionAsync(int marketId, AddScheduleExceptionRequest req)
        {
            if (!await db.Markets.AnyAsync(m => m.Id == marketId))
                return (false, "Η αγορά δεν βρέθηκε.");

            db.MarketSchedules.Add(new market_schedule
            {
                MarketId = marketId,
                ExceptionDate = req.ExceptionDate,
                IsCancelled = req.IsCancelled,
                CancellationReason = req.CancellationReason
            });

            await db.SaveChangesAsync();
            return (true, null);
        }

        // ── Helper ────────────────────────────────────────────────────────────
        private static MarketDto ToDto(market m) => new(
            m.Id, m.Name, m.MarketType, m.Address,
            m.Latitude, m.Longitude,
            m.TotalSpots,
            m.MarketSellers.Count(ms => ms.IsActive),   // OccupiedSpots
            m.OpenTime, m.CloseTime, m.Notes, m.IsActive,
            m.Schedules.Select(s => new MarketScheduleDto(
                s.Id, s.Day, s.ExceptionDate, s.IsCancelled, s.CancellationReason)).ToList(),
            m.MarketSellers.Select(ms => new MarketSellerDto(
                ms.SellerId,
                $"{ms.Seller.FirstName} {ms.Seller.LastName}",
                ms.SpotNumber, ms.SpotLength, ms.IsActive)).ToList());
    }
}
