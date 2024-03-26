using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api.Data.config;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id);

        builder.Property(c => c.Title)
            .HasColumnType("NVARCHAR")
            .HasMaxLength(280)
            .IsRequired();

        builder.Property(c => c.Content)
            .HasColumnType("NVARCHAR")
            .HasMaxLength(280);

        builder.Property(c => c.CreatedOn)
            .IsRequired();

        builder.HasOne(c => c.Stock)
            .WithMany(s => s.Comments)
            .HasForeignKey(c => c.Id)
            .IsRequired(false);

        builder.HasOne(c => c.AppUser)
            .WithMany(a => a.Comments)
            .HasForeignKey(c => c.AppUserId)
            .IsRequired();

        builder.ToTable("Comments");
    }
}