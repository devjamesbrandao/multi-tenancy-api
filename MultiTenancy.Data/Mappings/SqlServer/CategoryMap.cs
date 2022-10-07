using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiTenancy.Domain.Entities;

namespace MultiTenancy.Data.Mappings.SqlServer;

public class CategoryMap : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Category");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .HasMaxLength(100);

        builder.HasData(new[]
        {
            new Category(1, "Eletrodomésticos"),
            new Category(2, "Informática"),
            new Category(3, "Vestuário"),
            new Category(4, "Livros")
        });
    }
}