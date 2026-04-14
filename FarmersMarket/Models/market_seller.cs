namespace FarmersMarket.Models
{
    public class market_seller
    {
        public int MarketId { get; set; }
        public market Market { get; set; } = null!;

        public int SellerId { get; set; }
        public seller Seller { get; set; } = null!;

        public int SpotNumber { get; set; }        // Αριθμός θέσης
        public decimal SpotLength { get; set; }    // Μήκος πάγκου (μέτρα)

        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
    }
}

