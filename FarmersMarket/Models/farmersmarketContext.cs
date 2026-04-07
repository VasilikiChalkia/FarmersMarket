using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FarmersMarket.Models;

namespace FarmersMarket.Models;

public class farmersmarketContext(DbContextOptions<farmersmarketContext> options)
    : IdentityDbContext<User>(options)   // ← User αντί για AppUser
{
    // Τα υπόλοιπα DbSet<> σου εδώ, αμετάβλητα
    // public DbSet<Market>  Markets  { get; set; }
    // public DbSet<License> Licenses { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder); // ← ΑΠΑΡΑΙΤΗΤΟ πρώτο

        // Οι δικοί σου fluent κανόνες κάτω από το base
    }
}