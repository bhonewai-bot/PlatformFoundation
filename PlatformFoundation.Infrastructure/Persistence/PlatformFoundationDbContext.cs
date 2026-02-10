using Microsoft.EntityFrameworkCore;
using PlatformFoundation.Domain.Entities;
using PlatformFoundation.Infrastructure.AppDbContext.Configurations;

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

        modelBuilder.ApplyConfiguration(new ProductConfiguration());
    }
}
