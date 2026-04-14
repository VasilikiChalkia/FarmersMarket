using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FarmersMarket.Models;

public class farmersmarketContext(DbContextOptions<farmersmarketContext> options)
    : IdentityDbContext<User>(options)
{
    // Sellers
    public DbSet<seller> Sellers => Set<seller>();
    public DbSet<seller_license> SellerLicenses => Set<seller_license>();

    // Markets
    public DbSet<market> Markets => Set<market>();
    public DbSet<market_schedule> MarketSchedules => Set<market_schedule>();
    public DbSet<market_seller> MarketSellers => Set<market_seller>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // ── Seller ────────────────────────────────────────────────────────
        builder.Entity<seller>(e =>
        {
            e.HasKey(s => s.Id);
            e.Property(s => s.FirstName).IsRequired().HasMaxLength(100);
            e.Property(s => s.LastName).IsRequired().HasMaxLength(100);
            e.Property(s => s.Afm).IsRequired().HasMaxLength(9);
            e.Property(s => s.Email).IsRequired().HasMaxLength(200);
            e.Property(s => s.Phone).IsRequired().HasMaxLength(20);
            e.Property(s => s.Address).HasMaxLength(300);
            e.HasIndex(s => s.Afm).IsUnique();
            e.Property(s => s.SellerType).HasConversion<string>();

            e.HasMany(s => s.Licenses)
             .WithOne(l => l.Seller)
             .HasForeignKey(l => l.SellerId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasMany(s => s.MarketSellers) 
              .WithOne(ms => ms.Seller)     
              .HasForeignKey(ms => ms.SellerId)
              .OnDelete(DeleteBehavior.Cascade);
        });

        // ── SellerLicense ─────────────────────────────────────────────────
        builder.Entity<seller_license>(e =>
        {
            e.HasKey(l => l.Id);
            e.Property(l => l.LicenseNumber).IsRequired().HasMaxLength(50);
            e.Property(l => l.LicenseType).IsRequired().HasMaxLength(100);
            e.Property(l => l.Status).HasConversion<string>();
        });

        // ── Market ────────────────────────────────────────────────────────
        builder.Entity<market>(e =>
        {
            e.HasKey(m => m.Id);
            e.Property(m => m.Name).IsRequired().HasMaxLength(200);
            e.Property(m => m.Address).IsRequired().HasMaxLength(300);
            e.Property(m => m.Latitude).HasPrecision(9, 6);
            e.Property(m => m.Longitude).HasPrecision(9, 6);
            e.Property(m => m.Notes).HasMaxLength(1000);
            e.Property(m => m.MarketType).HasConversion<string>();

            e.HasMany(m => m.Schedules)
             .WithOne(s => s.Market)
             .HasForeignKey(s => s.MarketId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasMany(m => m.MarketSellers)
             .WithOne(ms => ms.Market)
             .HasForeignKey(ms => ms.MarketId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ── MarketSchedule ────────────────────────────────────────────────
        builder.Entity<market_schedule>(e =>
        {
            e.HasKey(s => s.Id);
            e.Property(s => s.Day).HasConversion<string>();
            e.Property(s => s.CancellationReason).HasMaxLength(500);
        });

        // ── MarketSeller ──────────────────────────────────────────────────
        builder.Entity<market_seller>(e =>
        {
            e.HasKey(ms => new { ms.MarketId, ms.SellerId });
            e.Property(ms => ms.SpotLength).HasPrecision(5, 2);
        });
    }
}