using static FarmersMarket.Features.Markets.MarketEnums;
using static FarmersMarket.Features.Markets.MarketsDTO;

namespace FarmersMarket.Models
{
    public class market
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;           // Ονομασία αγοράς
        public MarketType MarketType { get; set; }

        // Τοποθεσία
        public string Address { get; set; } = string.Empty;
        public decimal? Latitude { get; set; }                     // Συντεταγμένες
        public decimal? Longitude { get; set; }

        // Χαρακτηριστικά
        public int TotalSpots { get; set; }                        // Διαθέσιμες θέσεις
        public TimeOnly OpenTime { get; set; }                     // Ωράριο έναρξης
        public TimeOnly CloseTime { get; set; }                    // Ωράριο λήξης
        public string? Notes { get; set; }                         // Ειδικές ρυθμίσεις

        public bool IsActive { get; set; } = true;

        // Audit
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation
        public ICollection<market_schedule> Schedules { get; set; } = [];
        public ICollection<market_seller> MarketSellers { get; set; } = [];
    }
}
