using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static FarmersMarket.Features.Markets.MarketsDTO;

namespace FarmersMarket.Features.Markets
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MarketsController(IMarketService marketService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] MarketQueryParams query)
            => Ok(await marketService.GetAllAsync(query));

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
            => await marketService.GetByIdAsync(id) is { } dto ? Ok(dto) : NotFound();

        [HttpPost]
        [Authorize(Roles = "SystemAdmin")]
        public async Task<IActionResult> Create(CreateMarketRequest req)
        {
            var (success, id, error) = await marketService.CreateAsync(req);
            return success
                ? CreatedAtAction(nameof(GetById), new { id }, null)
                : BadRequest(new { error });
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "SystemAdmin")]
        public async Task<IActionResult> Update(int id, UpdateMarketRequest req)
        {
            var (success, error) = await marketService.UpdateAsync(id, req);
            return success ? NoContent() : BadRequest(new { error });
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "SystemAdmin")]
        public async Task<IActionResult> Delete(int id)
        {
            var (success, error) = await marketService.DeleteAsync(id);
            return success ? NoContent() : BadRequest(new { error });
        }

        // Πωλητές αγοράς
        [HttpPost("{id:int}/sellers")]
        [Authorize(Roles = "SystemAdmin")]
        public async Task<IActionResult> AssignSeller(int id, AssignSellerRequest req)
        {
            var (success, error) = await marketService.AssignSellerAsync(id, req);
            return success ? NoContent() : BadRequest(new { error });
        }

        [HttpDelete("{id:int}/sellers/{sellerId:int}")]
        [Authorize(Roles = "SystemAdmin")]
        public async Task<IActionResult> RemoveSeller(int id, int sellerId)
        {
            var (success, error) = await marketService.RemoveSellerAsync(id, sellerId);
            return success ? NoContent() : BadRequest(new { error });
        }

        // Εξαιρέσεις προγράμματος (αργίες κ.λπ.)
        [HttpPost("{id:int}/schedule-exceptions")]
        [Authorize(Roles = "SystemAdmin")]
        public async Task<IActionResult> AddScheduleException(int id, AddScheduleExceptionRequest req)
        {
            var (success, error) = await marketService.AddScheduleExceptionAsync(id, req);
            return success ? NoContent() : BadRequest(new { error });
        }
    }
}
