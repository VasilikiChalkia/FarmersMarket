using static FarmersMarket.Features.Sellers.SellerEnums;

namespace FarmersMarket.Models
{
    public class seller
    {
        public int Id { get; set; }

        // Δημογραφικά
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Afm { get; set; } = string.Empty;         // ΑΦΜ
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string? Address { get; set; }

        // Επαγγελματική ιδιότητα
        public SellerType SellerType { get; set; }

        // Κατάσταση
        public bool IsActive { get; set; } = true;

        // Audit
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Σύνδεση με User (προαιρετικό - αν κάνει login μέσω taxisnet)
        public string? UserId { get; set; }
        public User? User { get; set; }

        // Navigation
        public ICollection<seller_license> Licenses { get; set; } = [];

        public ICollection<market_seller> MarketSellers { get; set; } = new List<market_seller>();
    }
}
