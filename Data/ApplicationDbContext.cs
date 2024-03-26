using api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace api.Data;

public class ApplicationDbContext(DbContextOptions dbContextOptions) : IdentityDbContext<AppUser>(dbContextOptions)
{
    public DbSet<Stock> Stocks { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Portfolio> Portfolios { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        // builder.Entity<Portfolio>(item => item.HasKey(p => new { p.AppUserId, p.StockId }));

        // builder.Entity<Portfolio>()
        //     .HasOne(portfolio => portfolio.AppUser)
        //     .WithMany(appUser => appUser.Portfolios)
        //     .HasForeignKey(portfolio => portfolio.AppUserId);

        // builder.Entity<Portfolio>()
        //     .HasOne(portfolio => portfolio.Stock)
        //     .WithMany(stock => stock.Portfolios)
        //     .HasForeignKey(portfolio => portfolio.StockId);

        // List<IdentityRole> roles =
        // [
        //     new IdentityRole
        //     {
        //         Name = "Admin",
        //         NormalizedName = "ADMIN",
        //     },
        //     new IdentityRole
        //     {
        //         Name = "User",
        //         NormalizedName = "USER",
        //     },
        // ];

        // builder.Entity<IdentityRole>().HasData(roles);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var connectionString = config["ConnectionStrings:DefaultConnection"];

        if (connectionString.IsNullOrEmpty())
            throw new NullReferenceException();

        optionsBuilder.UseSqlServer(connectionString);
    }
}