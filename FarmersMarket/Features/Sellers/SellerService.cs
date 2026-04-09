using FarmersMarket.Models;
using Microsoft.EntityFrameworkCore;
using static FarmersMarket.Features.Sellers.SellerEnums;
using static FarmersMarket.Features.Sellers.SellersDTOs;

namespace FarmersMarket.Features.Sellers
{
    public class SellerService(farmersmarketContext db) : ISellerService
    {
        public async Task<PagedResult<SellerDto>> GetAllAsync(SellerQueryParams q)
        {
            var query = db.Sellers.Include(s => s.Licenses).AsQueryable();

            if (!string.IsNullOrWhiteSpace(q.Name))
                query = query.Where(s =>
                    s.FirstName.Contains(q.Name) || s.LastName.Contains(q.Name));

            if (!string.IsNullOrWhiteSpace(q.Afm))
                query = query.Where(s => s.Afm == q.Afm);

            if (q.SellerType.HasValue)
                query = query.Where(s => s.SellerType == q.SellerType.Value);

            if (q.IsActive.HasValue)
                query = query.Where(s => s.IsActive == q.IsActive.Value);

            var total = await query.CountAsync();
            var items = await query
                .OrderBy(s => s.LastName).ThenBy(s => s.FirstName)
                .Skip((q.Page - 1) * q.PageSize)
                .Take(q.PageSize)
                .Select(s => ToDto(s))
                .ToListAsync();

            return new PagedResult<SellerDto>(items, total, q.Page, q.PageSize);
        }

        public async Task<SellerDto?> GetByIdAsync(int id)
        {
            var seller = await db.Sellers.Include(s => s.Licenses)
                .FirstOrDefaultAsync(s => s.Id == id);
            return seller is null ? null : ToDto(seller);
        }

        public async Task<(bool Success, int? Id, string? Error)> CreateAsync(CreateSellerRequest req)
        {
            if (await db.Sellers.AnyAsync(s => s.Afm == req.Afm))
                return (false, null, "Υπάρχει ήδη πωλητής με αυτό το ΑΦΜ.");

            var seller = new seller
            {
                FirstName = req.FirstName,
                LastName = req.LastName,
                Afm = req.Afm,
                Email = req.Email,
                Phone = req.Phone,
                Address = req.Address,
                SellerType = req.SellerType
            };

            db.Sellers.Add(seller);
            await db.SaveChangesAsync();
            return (true, seller.Id, null);
        }

        public async Task<(bool Success, string? Error)> UpdateAsync(int id, UpdateSellerRequest req)
        {
            var seller = await db.Sellers.FindAsync(id);
            if (seller is null) return (false, "Ο πωλητής δεν βρέθηκε.");

            seller.FirstName = req.FirstName;
            seller.LastName = req.LastName;
            seller.Email = req.Email;
            seller.Phone = req.Phone;
            seller.Address = req.Address;
            seller.SellerType = req.SellerType;
            seller.IsActive = req.IsActive;
            seller.UpdatedAt = DateTime.UtcNow;

            await db.SaveChangesAsync();
            return (true, null);
        }

        public async Task<(bool Success, string? Error)> DeleteAsync(int id)
        {
            var seller = await db.Sellers.FindAsync(id);
            if (seller is null) return (false, "Ο πωλητής δεν βρέθηκε.");

            db.Sellers.Remove(seller);
            await db.SaveChangesAsync();
            return (true, null);
        }

        public async Task<(bool Success, string? Error)> AddLicenseAsync(int sellerId, AddLicenseRequest req)
        {
            if (!await db.Sellers.AnyAsync(s => s.Id == sellerId))
                return (false, "Ο πωλητής δεν βρέθηκε.");

            var license = new seller_license
            {
                SellerId = sellerId,
                LicenseNumber = req.LicenseNumber,
                LicenseType = req.LicenseType,
                IssuedAt = req.IssuedAt,
                ExpiresAt = req.ExpiresAt
            };

            db.SellerLicenses.Add(license);
            await db.SaveChangesAsync();
            return (true, null);
        }

        public async Task<(bool Success, string? Error)> UpdateLicenseStatusAsync(int licenseId, LicenseStatus status)
        {
            var license = await db.SellerLicenses.FindAsync(licenseId);
            if (license is null) return (false, "Η άδεια δεν βρέθηκε.");

            license.Status = status;
            await db.SaveChangesAsync();
            return (true, null);
        }

        // ── Helper ────────────────────────────────────────────────────────────
        private static SellerDto ToDto(seller s) => new(
            s.Id, s.FirstName, s.LastName, s.Afm, s.Email, s.Phone, s.Address,
            s.SellerType, s.IsActive, s.CreatedAt,
            s.Licenses.Select(l => new SellerLicenseDto(
                l.Id, l.LicenseNumber, l.LicenseType,
                l.Status, l.IssuedAt, l.ExpiresAt)).ToList());
    }
}
