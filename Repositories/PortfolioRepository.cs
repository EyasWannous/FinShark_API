using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories;

public class PortfolioRepository(ApplicationDbContext context) : IPortfolioRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Portfolio> CreateAsync(Portfolio portfolio)
    {
        await _context.Portfolios.AddAsync(portfolio);

        await _context.SaveChangesAsync();

        return portfolio;
    }

    public async Task<List<Stock>> GetUserPortfolioAsync(AppUser appUser)
    {
        return await _context.Portfolios
            .Where(portfolio => portfolio.AppUserId == appUser.Id)
            .Select(portfolio => new Stock
            {
                Id = portfolio.StockId,
                Symbol = portfolio.Stock.Symbol,
                CompanyName = portfolio.Stock.CompanyName,
                PurchasePrice = portfolio.Stock.PurchasePrice,
                LastDiv = portfolio.Stock.LastDiv,
                Industry = portfolio.Stock.Industry,
                MarketCap = portfolio.Stock.MarketCap,
            })
            .ToListAsync();
    }

    public async Task<Portfolio?> DeleteAsync(AppUser appUser, string symbol)
    {
        var portfolioModel = await _context.Portfolios
            .FirstOrDefaultAsync
            (portfolio
                => portfolio.AppUserId == appUser.Id
                && portfolio.Stock.Symbol.Equals(symbol.Trim(), StringComparison.CurrentCultureIgnoreCase
            ));
        if (portfolioModel is null)
            return null;

        _context.Portfolios.Remove(portfolioModel);

        await _context.SaveChangesAsync();

        return portfolioModel;
    }
}