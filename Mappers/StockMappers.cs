using api.DTOs.Stock;
using api.Models;

namespace api.Mappers;

public static class StockMappers
{
    public static StockDTO ToStockDTO(this Stock stockModel)
    {
        return new StockDTO
        {
            Id = stockModel.Id,
            Symbol = stockModel.Symbol,
            CompanyName = stockModel.CompanyName,
            PurchasePrice = stockModel.PurchasePrice,
            LastDiv = stockModel.LastDiv,
            Industry = stockModel.Industry,
            MarketCap = stockModel.MarketCap,
            Comments = stockModel.Comments.Select(comment => comment.ToCommentDTO()).ToList(),
        };
    }

    public static Stock ToStockFromCreateDTO(this CreateStockRequestDTO stockModel)
    {
        return new Stock
        {
            Symbol = stockModel.Symbol,
            CompanyName = stockModel.CompanyName,
            Industry = stockModel.Industry,
            LastDiv = stockModel.LastDiv,
            MarketCap = stockModel.MarketCap,
            PurchasePrice = stockModel.PurchasePrice,
        };
    }
}