using Microsoft.AspNetCore.Identity;
using FarmersMarket.Features.Users.Dtos;
using FarmersMarket.Models;

namespace FarmersMarket.Features.Users.Services;

public class UserService(UserManager<User> userManager) : IUserService
{
    public async Task<IList<UserDto>> GetAllAsync()
    {
        var users = userManager.Users.ToList();

        var result = new List<UserDto>();
        foreach (var u in users)
        {
            var roles = await userManager.GetRolesAsync(u);
            result.Add(ToDto(u, roles));
        }
        return result;
    }

    public async Task<UserDto?> GetByIdAsync(string id)
    {
        var user = await userManager.FindByIdAsync(id);
        if (user is null) return null;

        var roles = await userManager.GetRolesAsync(user);
        return ToDto(user, roles);
    }

    public async Task<(bool Success, string? Error)> UpdateAsync(string id, UpdateUserRequest req)
    {
        var user = await userManager.FindByIdAsync(id);
        if (user is null) return (false, "Ο χρήστης δεν βρέθηκε.");

        user.FirstName = req.FirstName;
        user.LastName = req.LastName;
        user.IsActive = req.IsActive;

        var result = await userManager.UpdateAsync(user);
        return result.Succeeded
            ? (true, null)
            : (false, string.Join(", ", result.Errors.Select(e => e.Description)));
    }

    public async Task<(bool Success, string? Error)> DeleteAsync(string id)
    {
        var user = await userManager.FindByIdAsync(id);
        if (user is null) return (false, "Ο χρήστης δεν βρέθηκε.");

        var result = await userManager.DeleteAsync(user);
        return result.Succeeded
            ? (true, null)
            : (false, string.Join(", ", result.Errors.Select(e => e.Description)));
    }

    public async Task<(bool Success, string? Error)> AssignRoleAsync(string id, string role)
    {
        if (!AppRoles.All.Contains(role))
            return (false, $"Μη έγκυρος ρόλος: {role}");

        var user = await userManager.FindByIdAsync(id);
        if (user is null) return (false, "Ο χρήστης δεν βρέθηκε.");

        if (await userManager.IsInRoleAsync(user, role))
            return (false, "Ο χρήστης έχει ήδη αυτόν τον ρόλο.");

        var result = await userManager.AddToRoleAsync(user, role);
        return result.Succeeded
            ? (true, null)
            : (false, string.Join(", ", result.Errors.Select(e => e.Description)));
    }

    public async Task<(bool Success, string? Error)> RemoveRoleAsync(string id, string role)
    {
        var user = await userManager.FindByIdAsync(id);
        if (user is null) return (false, "Ο χρήστης δεν βρέθηκε.");

        var result = await userManager.RemoveFromRoleAsync(user, role);
        return result.Succeeded
            ? (true, null)
            : (false, string.Join(", ", result.Errors.Select(e => e.Description)));
    }

    // ── Helper ────────────────────────────────────────────────────────────────
    private static UserDto ToDto(User u, IList<string> roles) => new(
        u.Id, u.Email!, u.FirstName, u.LastName, u.IsActive, u.CreatedAt, roles);
}