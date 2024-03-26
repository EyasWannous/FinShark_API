using api.DTOs.Stock;
using api.Helpers;
using api.Models;

namespace api.Repositories;

public interface IStockRepository
{
    Task<List<Stock>> GetAllAsync(QueryObject queryObject);

    Task<Stock?> GetByIdAsync(int id);

    Task<Stock?> GetBySymbolAsync(string symbol);

    Task<Stock> CreateAsync(Stock stockModel);

    Task<Stock?> UpdateAsync(int id, UpdateStockRequestDTO updateDTO);

    Task<Stock?> DeleteAsync(int id);

    Task<bool> StockExists(int id);

}