using static FarmersMarket.Features.Markets.MarketsDTO;
using static FarmersMarket.Features.Sellers.SellersDTOs;

namespace FarmersMarket.Features.Markets
{
    public interface IMarketService
    {
        Task<PagedResult<MarketDto>> GetAllAsync(MarketQueryParams query);
        Task<MarketDto?> GetByIdAsync(int id);
        Task<(bool Success, int? Id, string? Error)> CreateAsync(CreateMarketRequest req);
        Task<(bool Success, string? Error)> UpdateAsync(int id, UpdateMarketRequest req);
        Task<(bool Success, string? Error)> DeleteAsync(int id);

        // Πωλητές
        Task<(bool Success, string? Error)> AssignSellerAsync(int marketId, AssignSellerRequest req);
        Task<(bool Success, string? Error)> RemoveSellerAsync(int marketId, int sellerId);

        // Εξαιρέσεις προγράμματος
        Task<(bool Success, string? Error)> AddScheduleExceptionAsync(int marketId, AddScheduleExceptionRequest req);
    }
}
