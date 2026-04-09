namespace FarmersMarket.Features.Sellers
{
    public class SellerEnums
    {
        public enum SellerType
        {
            Pro2ducer,       // Παραγωγός
            Professional,   // Επαγγελματίας
            Seasonal        // Εποχικός
        }

        public enum LicenseStatus
        {
            Active,     // Ενεργή
            Expired,    // Ληγμένη
            Suspended,  // Ανασταλμένη
            Revoked     // Ανακληθείσα
        }
    }
}
