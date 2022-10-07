using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiTenancy.Domain.Entities;

namespace MultiTenancy.Data.Mappings.PostGres;

public class ProductMap : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Product");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Code)
            .HasMaxLength(6);
        
        builder.Property(p => p.Name)
            .HasMaxLength(100);

        builder.Property(p => p.Price)
            .HasPrecision(18, 2);

        builder.HasOne(p => p.Category)
            .WithMany(p => p.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}