using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static FarmersMarket.Features.Sellers.SellerEnums;
using static FarmersMarket.Features.Sellers.SellersDTOs;

namespace FarmersMarket.Features.Sellers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SellersController(ISellerService sellerService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] SellerQueryParams query)
            => Ok(await sellerService.GetAllAsync(query));

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
            => await sellerService.GetByIdAsync(id) is { } dto ? Ok(dto) : NotFound();

        [HttpPost]
        [Authorize(Roles = "SystemAdmin")]
        public async Task<IActionResult> Create(CreateSellerRequest req)
        {
            var (success, id, error) = await sellerService.CreateAsync(req);
            return success
                ? CreatedAtAction(nameof(GetById), new { id }, null)
                : BadRequest(new { error });
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "SystemAdmin")]
        public async Task<IActionResult> Update(int id, UpdateSellerRequest req)
        {
            var (success, error) = await sellerService.UpdateAsync(id, req);
            return success ? NoContent() : BadRequest(new { error });
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "SystemAdmin")]
        public async Task<IActionResult> Delete(int id)
        {
            var (success, error) = await sellerService.DeleteAsync(id);
            return success ? NoContent() : BadRequest(new { error });
        }

        // Άδειες
        [HttpPost("{id:int}/licenses")]
        [Authorize(Roles = "SystemAdmin")]
        public async Task<IActionResult> AddLicense(int id, AddLicenseRequest req)
        {
            var (success, error) = await sellerService.AddLicenseAsync(id, req);
            return success ? NoContent() : BadRequest(new { error });
        }

        [HttpPatch("licenses/{licenseId:int}/status")]
        [Authorize(Roles = "SystemAdmin")]
        public async Task<IActionResult> UpdateLicenseStatus(int licenseId, [FromBody] LicenseStatus status)
        {
            var (success, error) = await sellerService.UpdateLicenseStatusAsync(licenseId, status);
            return success ? NoContent() : BadRequest(new { error });
        }
    }
}