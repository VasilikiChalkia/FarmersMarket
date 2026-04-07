namespace FarmersMarket.Features.Users.Dtos;

// ── Auth ──────────────────────────────────────────────────────────────────────

public record RegisterRequest(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    string Role = "Seller"          // default ρόλος
);

public record LoginRequest(
    string Email,
    string Password
);

public record AuthResponse(
    string Token,
    string Email,
    IList<string> Roles
);

// ── User Management (για admin) ───────────────────────────────────────────────

public record UserDto(
    string Id,
    string Email,
    string FirstName,
    string LastName,
    bool IsActive,
    DateTime CreatedAt,
    IList<string> Roles
);

public record UpdateUserRequest(
    string FirstName,
    string LastName,
    bool IsActive
);

public record AssignRoleRequest(string Role);