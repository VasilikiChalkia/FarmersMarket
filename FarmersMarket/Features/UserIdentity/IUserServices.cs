using FarmersMarket.Features.Users.Dtos;

namespace FarmersMarket.Features.Users.Services;

public interface IAuthService
{
    Task<(bool Success, string? Error)> RegisterAsync(RegisterRequest request);
    Task<(AuthResponse? Response, string? Error)> LoginAsync(LoginRequest request);
}

public interface IUserService
{
    Task<IList<UserDto>> GetAllAsync();
    Task<UserDto?> GetByIdAsync(string id);
    Task<(bool Success, string? Error)> UpdateAsync(string id, UpdateUserRequest request);
    Task<(bool Success, string? Error)> DeleteAsync(string id);
    Task<(bool Success, string? Error)> AssignRoleAsync(string id, string role);
    Task<(bool Success, string? Error)> RemoveRoleAsync(string id, string role);
}