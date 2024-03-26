using api.Extensions;
using api.Interfaces;
using api.Models;
using api.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PortfolioController(UserManager<AppUser> userManager, IStockRepository stockRepository
    , IPortfolioRepository portfolioRepository) : ControllerBase
{
    private readonly UserManager<AppUser> _userManager = userManager;
    private readonly IStockRepository _stockRepository = stockRepository;
    private readonly IPortfolioRepository _portfolioRepository = portfolioRepository;


    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetUserPortfolio()
    {
        var username = User.GetUsername();
        if (username is null)
            return BadRequest("User Not Found");


        var appUser = await _userManager.FindByNameAsync(username);
        if (appUser is null)
            return BadRequest();

        var userPortfolio = await _portfolioRepository.GetUserPortfolioAsync(appUser);

        return Ok(userPortfolio);
    }


    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddPortfolio(string symbol)
    {
        var username = User.GetUsername();
        // var username = User.Identity?.Name;
        if (username is null)
            return BadRequest("User Not Found");

        var appUser = await _userManager.FindByNameAsync(username);
        if (appUser is null)
            return BadRequest("AppUser Not Found In Database");

        var stock = await _stockRepository.GetBySymbolAsync(symbol);
        if (stock is null)
            return BadRequest("Stock Not Found");

        var userPortfolio = await _portfolioRepository.GetUserPortfolioAsync(appUser);
        if (userPortfolio.Any(p => p.Symbol.Equals(symbol.Trim(), StringComparison.CurrentCultureIgnoreCase)))
            return BadRequest("Cannot add same Stock to Portfolio");

        var portfolioModel = new Portfolio
        {
            StockId = stock.Id,
            AppUserId = appUser.Id,
            AppUser = appUser,
            Stock = stock,
        };

        await _portfolioRepository.CreateAsync(portfolioModel);

        if (portfolioModel is null)
            return StatusCode(500, "Could not Create");

        return Created();
    }

    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> DeletePortfolio(string symbol)
    {
        var username = User.GetUsername();
        if (username is null)
            return BadRequest("User Not Found");

        var appuser = await _userManager.FindByNameAsync(username);
        if (appuser is null)
            return BadRequest();

        var userPortfolio = await _portfolioRepository.GetUserPortfolioAsync(appuser);

        var filterdPortfolio = userPortfolio.Where(portfoilo => portfoilo.Symbol.Equals(symbol.Trim(), StringComparison.CurrentCultureIgnoreCase));

        if (filterdPortfolio.Count() != 1)
            return BadRequest("Stock not in your Portfolio");

        var deletePortfolio = await _portfolioRepository.DeleteAsync(appuser, symbol);
        if (deletePortfolio is null)
            return NotFound();

        return NoContent();
    }

}