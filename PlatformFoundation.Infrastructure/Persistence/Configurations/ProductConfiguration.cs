using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlatformFoundation.Domain.Entities;

namespace PlatformFoundation.Infrastructure.AppDbContext.Configurations;

public sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("products");
        
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();
        
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(x => x.Price)
            .HasPrecision(18, 2)
            .IsRequired();
        
        builder.ToTable(t => t.HasCheckConstraint("ck_products_price_gt_0", "\"Price\" > 0"));
    }
}
