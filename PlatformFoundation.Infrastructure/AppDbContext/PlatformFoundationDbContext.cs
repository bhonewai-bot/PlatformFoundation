using Microsoft.EntityFrameworkCore;
using PlatformFoundation.Domain.Entities;

namespace PlatformFoundation.Infrastructure.AppDbContext;

public sealed class PlatformFoundationDbContext : DbContext
{
    public PlatformFoundationDbContext(DbContextOptions<PlatformFoundationDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).IsRequired().HasMaxLength(100);
            entity.Property(x => x.Price).HasColumnType("decimal(18,2)");
        });
    }
}
