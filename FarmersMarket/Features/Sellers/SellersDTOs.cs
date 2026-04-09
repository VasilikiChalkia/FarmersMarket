using static FarmersMarket.Features.Sellers.SellerEnums;

namespace FarmersMarket.Features.Sellers
{
    public class SellersDTOs
    {
        public record SellerDto(
    int Id,
    string FirstName,
    string LastName,
    string Afm,
    string Email,
    string Phone,
    string? Address,
    SellerType SellerType,
    bool IsActive,
    DateTime CreatedAt,
    IList<SellerLicenseDto> Licenses
);

        public record SellerLicenseDto(
            int Id,
            string LicenseNumber,
            string LicenseType,
            LicenseStatus Status,
            DateOnly IssuedAt,
            DateOnly ExpiresAt
        );

        public record CreateSellerRequest(
            string FirstName,
            string LastName,
            string Afm,
            string Email,
            string Phone,
            string? Address,
            SellerType SellerType
        );

        public record UpdateSellerRequest(
            string FirstName,
            string LastName,
            string Email,
            string Phone,
            string? Address,
            SellerType SellerType,
            bool IsActive
        );

        public record AddLicenseRequest(
        string LicenseNumber,
        string LicenseType,
        DateOnly IssuedAt,
        DateOnly ExpiresAt
    );

        // Φίλτρα αναζήτησης (πολυκριτηριακή)
        public record SellerQueryParams(
            string? Name,
            string? Afm,
            SellerType? SellerType,
            bool? IsActive,
            int Page = 1,
            int PageSize = 20
        );

        public record PagedResult<T>(
            IList<T> Items,
            int TotalCount,
            int Page,
            int PageSize
        );
    }
}
