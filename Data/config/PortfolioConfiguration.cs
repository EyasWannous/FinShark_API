using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api.Data.config;

public class PortfolioConfiguration : IEntityTypeConfiguration<Portfolio>
{
    public void Configure(EntityTypeBuilder<Portfolio> builder)
    {
        builder.HasKey(p => new { p.AppUserId, p.StockId });

        builder.Property(p => p.AppUserId)
            .IsRequired();

        builder.Property(p => p.StockId)
            .IsRequired();

        builder.HasOne(p => p.AppUser)
            .WithMany(a => a.Portfolios)
            .HasForeignKey(p => p.AppUserId);

        builder.HasOne(p => p.Stock)
            .WithMany(s => s.Portfolios)
            .HasForeignKey(p => p.StockId);

        builder.ToTable("Portfolios");
    }
}