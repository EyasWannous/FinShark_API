using api.Models;

namespace api.Interfaces;

public interface IPortfolioRepository
{
    Task<List<Stock>> GetUserPortfolioAsync(AppUser appUser);

    Task<Portfolio> CreateAsync(Portfolio portfolio);

    Task<Portfolio?> DeleteAsync(AppUser appUser, string symbol);
}