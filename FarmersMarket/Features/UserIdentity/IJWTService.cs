using FarmersMarket.Models;

namespace FarmersMarket.Features.Users.Services;

public interface IJwtService
{
    string GenerateToken(User user, IList<string> roles);
}