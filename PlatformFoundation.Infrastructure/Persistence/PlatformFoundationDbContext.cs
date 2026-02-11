using Microsoft.EntityFrameworkCore;
using PlatformFoundation.Domain.Entities;
using PlatformFoundation.Infrastructure.Persistence.Configurations;

namespace PlatformFoundation.Infrastructure.Persistence;

public sealed class PlatformFoundationDbContext : DbContext
{
    public PlatformFoundationDbContext(DbContextOptions<PlatformFoundationDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductAuditLog> ProductAuditLogs => Set<ProductAuditLog>(); 

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new ProductConfiguration());
        modelBuilder.ApplyConfiguration(new ProductAuditLogConfiguration());
    }
}
