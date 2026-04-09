using static FarmersMarket.Features.Sellers.SellerEnums;
using static FarmersMarket.Features.Sellers.SellersDTOs;

namespace FarmersMarket.Features.Sellers
{
    public interface ISellerService
    {
        Task<PagedResult<SellerDto>> GetAllAsync(SellerQueryParams query);
        Task<SellerDto?> GetByIdAsync(int id);
        Task<(bool Success, int? Id, string? Error)> CreateAsync(CreateSellerRequest req);
        Task<(bool Success, string? Error)> UpdateAsync(int id, UpdateSellerRequest req);
        Task<(bool Success, string? Error)> DeleteAsync(int id);

        // Άδειες
        Task<(bool Success, string? Error)> AddLicenseAsync(int sellerId, AddLicenseRequest req);
        Task<(bool Success, string? Error)> UpdateLicenseStatusAsync(int licenseId, LicenseStatus status);
    }
}
