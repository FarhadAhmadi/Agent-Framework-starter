using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebAPI.Web.Domain.ProductAggregate;

namespace WebAPI.Web.Infrastructure.Data.Config;

public sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
  public void Configure(EntityTypeBuilder<Product> builder)
  {
    builder.Property(entity => entity.Id)
        .HasValueGenerator<VogenIntIdValueGenerator<AppDbContext, Product, ProductId>>()
        .HasVogenConversion()
        .ValueGeneratedOnAdd()
        .IsRequired();

    builder.Property(entity => entity.Name)
        .HasMaxLength(100)
        .IsRequired();

    builder.Property(entity => entity.UnitPrice)
        .HasPrecision(18, 2)
        .IsRequired();

    builder.HasData(
        new
        {
          Id = ProductId.From(1),
          Name = "Coffee Mug",
          UnitPrice = 9.99m
        },
        new
        {
          Id = ProductId.From(2),
          Name = "T-Shirt",
          UnitPrice = 19.99m
        },
        new
        {
          Id = ProductId.From(3),
          Name = "Sticker Pack",
          UnitPrice = 3.99m
        }
    );
  }
}

