using Microsoft.AspNetCore.Identity;
using FarmersMarket.Features.Users.Dtos;
using FarmersMarket.Models;

namespace FarmersMarket.Features.Users.Services;

public class AuthService(
    UserManager<User> userManager,
    IJwtService jwtService) : IAuthService
{
    public async Task<(bool Success, string? Error)> RegisterAsync(RegisterRequest req)
    {
        if (await userManager.FindByEmailAsync(req.Email) is not null)
            return (false, "Το email χρησιμοποιείται ήδη.");

        // Έλεγχος ότι ο ρόλος είναι έγκυρος
        if (!AppRoles.All.Contains(req.Role))
            return (false, $"Μη έγκυρος ρόλος: {req.Role}");

        var user = new User
        {
            UserName = req.Email,
            Email = req.Email,
            FirstName = req.FirstName,
            LastName = req.LastName,
        };

        var result = await userManager.CreateAsync(user, req.Password);
        if (!result.Succeeded)
            return (false, string.Join(", ", result.Errors.Select(e => e.Description)));

        await userManager.AddToRoleAsync(user, req.Role);
        return (true, null);
    }

    public async Task<(AuthResponse? Response, string? Error)> LoginAsync(LoginRequest req)
    {
        var user = await userManager.FindByEmailAsync(req.Email);

        if (user is null || !user.IsActive)
            return (null, "Λανθασμένα στοιχεία σύνδεσης.");

        if (!await userManager.CheckPasswordAsync(user, req.Password))
        {
            await userManager.AccessFailedAsync(user); // lockout μετρητής
            return (null, "Λανθασμένα στοιχεία σύνδεσης.");
        }

        if (await userManager.IsLockedOutAsync(user))
            return (null, "Ο λογαριασμός είναι κλειδωμένος. Δοκιμάστε αργότερα.");

        await userManager.ResetAccessFailedCountAsync(user);

        var roles = await userManager.GetRolesAsync(user);
        var token = jwtService.GenerateToken(user, roles);

        return (new AuthResponse(token, user.Email!, roles), null);
    }
}