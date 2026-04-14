using Microsoft.AspNetCore.Identity;

namespace FarmersMarket.Models;

// Το υπάρχον User.cs σου παίρνει όλα τα Identity fields δωρεάν:
// Id, UserName, Email, PasswordHash, PhoneNumber, LockoutEnd, κ.λπ.
public class User : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}