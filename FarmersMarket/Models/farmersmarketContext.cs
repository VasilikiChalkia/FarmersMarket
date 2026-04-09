using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace FarmersMarket.Models;

public class farmersmarketContext(DbContextOptions<farmersmarketContext> options)
    : IdentityDbContext<User>(options)
{
    public DbSet<seller> Sellers => Set<seller>();
    public DbSet<seller_license> SellerLicenses => Set<seller_license>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(farmersmarketContext).Assembly);
    }
}