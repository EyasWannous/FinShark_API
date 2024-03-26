using api.Data;
using api.DTOs.Stock;
using api.Helpers;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories;

public class StockRepository(ApplicationDbContext context) : IStockRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Stock> CreateAsync(Stock stockModel)
    {
        await _context.Stocks.AddAsync(stockModel);
        await _context.SaveChangesAsync();

        return stockModel;
    }

    public async Task<Stock?> DeleteAsync(int id)
    {
        var stockModel = await GetByIdAsync(id);
        if (stockModel is null)
            return null;

        _context.Stocks.Remove(stockModel);
        await _context.SaveChangesAsync();

        return stockModel;
    }

    public async Task<List<Stock>> GetAllAsync(QueryObject queryObject)
    {
        queryObject.TrimAll();
        var stocks = _context.Stocks
            .Include(stock => stock.Comments)
            .ThenInclude(comment => comment.AppUser)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(queryObject.CompanyName))
            stocks = stocks.Where(stock => stock.CompanyName.Contains(queryObject.CompanyName));

        if (!string.IsNullOrWhiteSpace(queryObject.Symbol))
            stocks = stocks.Where(stock => stock.Symbol.Contains(queryObject.Symbol));

        if (!string.IsNullOrWhiteSpace(queryObject.SortBy))
        {
            if (queryObject.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
            {
                stocks = queryObject.IsDecsending
                    ? stocks.OrderByDescending(stock => stock.Symbol)
                    : stocks.OrderBy(stock => stock.Symbol);
            }
        }

        var skipNumber = (queryObject.PageNumber - 1) * queryObject.PageSize;

        return await stocks
            .Skip(skipNumber)
            .Take(queryObject.PageSize)
            .ToListAsync();
    }

    public async Task<Stock?> GetByIdAsync(int id)
        => await _context.Stocks
            .Include(stock => stock.Comments)
            .ThenInclude(comment => comment.AppUser)
            .FirstOrDefaultAsync(stock => stock.Id == id);

    public async Task<Stock?> GetBySymbolAsync(string symbol)
        => await _context.Stocks.FirstOrDefaultAsync(stock => stock.Symbol == symbol);

    public async Task<bool> StockExists(int id)
        => await _context.Stocks.AnyAsync(stock => stock.Id == id);

    public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDTO updateDTO)
    {
        var stockModel = await GetByIdAsync(id);
        if (stockModel is null)
            return null;

        stockModel.Symbol = updateDTO.Symbol;
        stockModel.CompanyName = updateDTO.CompanyName;
        stockModel.Industry = updateDTO.Industry;
        stockModel.LastDiv = updateDTO.LastDiv;
        stockModel.MarketCap = updateDTO.MarketCap;
        stockModel.PurchasePrice = updateDTO.PurchasePrice;

        await _context.SaveChangesAsync();

        return stockModel;
    }
}