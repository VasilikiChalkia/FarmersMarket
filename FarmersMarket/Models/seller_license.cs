using static FarmersMarket.Features.Sellers.SellerEnums;

namespace FarmersMarket.Models
{
    public class seller_license
    {
        public int Id { get; set; }

        public int SellerId { get; set; }
        public seller Seller { get; set; } = null!;

        public string LicenseNumber { get; set; } = string.Empty;  // Αρ. άδειας
        public string LicenseType { get; set; } = string.Empty;    // Τύπος άδειας
        public LicenseStatus Status { get; set; } = LicenseStatus.Active;

        public DateOnly IssuedAt { get; set; }
        public DateOnly ExpiresAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
