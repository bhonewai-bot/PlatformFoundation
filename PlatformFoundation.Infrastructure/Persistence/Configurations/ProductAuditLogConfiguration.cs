using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlatformFoundation.Domain.Entities;

namespace PlatformFoundation.Infrastructure.Persistence.Configurations;

public sealed class ProductAuditLogConfiguration : IEntityTypeConfiguration<ProductAuditLog>
{
    public void Configure(EntityTypeBuilder<ProductAuditLog> builder)
    {
        builder.ToTable("product_audit_logs");

        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id).ValueGeneratedNever();
        
        builder.Property(x => x.ProductId).IsRequired();
        
        builder.Property(x => x.Action)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.HasIndex(x => x.ProductId);
    }
}
