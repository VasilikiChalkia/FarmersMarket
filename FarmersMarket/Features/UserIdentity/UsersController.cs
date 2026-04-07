using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FarmersMarket.Features.Users.Dtos;
using FarmersMarket.Features.Users.Services;
using FarmersMarket.Models;

namespace FarmersMarket.Features.Users.Controllers;

// ═══════════════════════════════════════════════════════════════════════════════
// AUTH — public endpoints (register / login)
// ═══════════════════════════════════════════════════════════════════════════════
[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    // POST api/auth/register
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest req)
    {
        var (success, error) = await authService.RegisterAsync(req);
        if (!success) return BadRequest(new { error });
        return Ok(new { message = "Εγγραφή επιτυχής." });
    }

    // POST api/auth/login
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest req)
    {
        var (response, error) = await authService.LoginAsync(req);
        if (response is null) return Unauthorized(new { error });
        return Ok(response);
    }
}

// ═══════════════════════════════════════════════════════════════════════════════
// USERS — διαχείριση χρηστών (μόνο SystemAdmin)
// ═══════════════════════════════════════════════════════════════════════════════
[ApiController]
[Route("api/users")]
/*[Authorize(Roles = AppRoles.SystemAdmin)] */  // ← όλο το controller κλειδωμένο
[Authorize]
public class UsersController(IUserService userService) : ControllerBase
{
    // GET api/users
    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await userService.GetAllAsync());

    // GET api/users/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var user = await userService.GetByIdAsync(id);
        return user is null ? NotFound() : Ok(user);
    }

    // PUT api/users/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateUserRequest req)
    {
        var (success, error) = await userService.UpdateAsync(id, req);
        if (!success) return BadRequest(new { error });
        return NoContent();
    }

    // DELETE api/users/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var (success, error) = await userService.DeleteAsync(id);
        if (!success) return BadRequest(new { error });
        return NoContent();
    }

    // POST api/users/{id}/roles
    [HttpPost("{id}/roles")]
    public async Task<IActionResult> AssignRole(string id, [FromBody] AssignRoleRequest req)
    {
        var (success, error) = await userService.AssignRoleAsync(id, req.Role);
        if (!success) return BadRequest(new { error });
        return Ok(new { message = $"Ρόλος '{req.Role}' ανατέθηκε." });
    }

    // DELETE api/users/{id}/roles/{role}
    [HttpDelete("{id}/roles/{role}")]
    public async Task<IActionResult> RemoveRole(string id, string role)
    {
        var (success, error) = await userService.RemoveRoleAsync(id, role);
        if (!success) return BadRequest(new { error });
        return Ok(new { message = $"Ρόλος '{role}' αφαιρέθηκε." });
    }
}