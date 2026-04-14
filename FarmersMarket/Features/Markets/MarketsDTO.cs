using static FarmersMarket.Features.Markets.MarketEnums;

namespace FarmersMarket.Features.Markets
{
    public class MarketsDTO
    {
       
        public record MarketDto(
    int Id,
    string Name,
    MarketType MarketType,
    string Address,
    decimal? Latitude,
    decimal? Longitude,
    int TotalSpots,
    int OccupiedSpots,
    TimeOnly OpenTime,
    TimeOnly CloseTime,
    string? Notes,
    bool IsActive,
    IList<MarketScheduleDto> Schedules,
    IList<MarketSellerDto> Sellers
);

        public record MarketScheduleDto(
            int Id,
            DayOfWeekGr Day,
            DateOnly? ExceptionDate,
            bool IsCancelled,
            string? CancellationReason
        );

        public record MarketSellerDto(
            int SellerId,
            string FullName,
            int SpotNumber,
            decimal SpotLength,
            bool IsActive
        );

        public record CreateMarketRequest(
            string Name,
            MarketType MarketType,
            string Address,
            decimal? Latitude,
            decimal? Longitude,
            int TotalSpots,
            TimeOnly OpenTime,
            TimeOnly CloseTime,
            string? Notes,
            IList<DayOfWeekGr> OperatingDays    // Ημέρες λειτουργίας
        );

        public record UpdateMarketRequest(
            string Name,
            MarketType MarketType,
            string Address,
            decimal? Latitude,
            decimal? Longitude,
            int TotalSpots,
            TimeOnly OpenTime,
            TimeOnly CloseTime,
            string? Notes,
            bool IsActive
        );

        public record AssignSellerRequest(
            int SellerId,
            int SpotNumber,
            decimal SpotLength
        );

        public record AddScheduleExceptionRequest(
            DateOnly ExceptionDate,
            bool IsCancelled,
            string? CancellationReason
        );

        public record MarketQueryParams(
            string? Name,
            MarketType? MarketType,
            DayOfWeekGr? Day,
            bool? IsActive,
            int Page = 1,
            int PageSize = 20
        );
    }
}
