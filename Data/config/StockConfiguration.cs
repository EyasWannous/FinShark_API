using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api.Data.config;

public class StockConfiguration : IEntityTypeConfiguration<Stock>
{
    public void Configure(EntityTypeBuilder<Stock> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id);

        builder.Property(s => s.Symbol)
            .HasColumnType("VARCHAR")
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(s => s.CompanyName)
            .HasColumnType("NVARCHAR")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(s => s.PurchasePrice)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(s => s.LastDiv)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(s => s.Industry)
            .HasColumnType("VARCHAR")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(s => s.MarketCap)
            .IsRequired();

        builder.HasMany(s => s.Comments)
            .WithOne(c => c.Stock)
            .HasForeignKey(c => c.StockId)
            .IsRequired(false);

        builder.HasMany(s => s.Portfolios)
            .WithOne(p => p.Stock)
            .HasForeignKey(p => p.StockId)
            .IsRequired();

        builder.ToTable("Stocks");
    }
}