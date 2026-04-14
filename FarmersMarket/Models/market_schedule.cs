using static FarmersMarket.Features.Markets.MarketEnums;
using static FarmersMarket.Features.Markets.MarketsDTO;

namespace FarmersMarket.Models
{
    public class market_schedule
    {
        public int Id { get; set; }

        public int MarketId { get; set; }
        public market Market { get; set; } = null!;

        public DayOfWeekGr Day { get; set; } 

        // Προσωρινές εξαιρέσεις (π.χ. αργίες)
        public DateOnly? ExceptionDate { get; set; }
        public bool IsCancelled { get; set; } = false;
        public string? CancellationReason { get; set; }
    }
}
