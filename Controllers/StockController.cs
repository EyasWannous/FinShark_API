using api.DTOs.Stock;
using api.Filters;
using api.Helpers;
using api.Mappers;
using api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StockController(IStockRepository stockRepository) : ControllerBase
{
    private readonly IStockRepository _stockRepository = stockRepository;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] QueryObject queryObject)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var stocks = await _stockRepository.GetAllAsync(queryObject);

        var stocksDTO = stocks.Select(stock => stock.ToStockDTO()).ToList();

        return Ok(stocksDTO);
    }


    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var stock = await _stockRepository.GetByIdAsync(id);

        if (stock is null)
            return NotFound();

        return Ok(stock.ToStockDTO());
    }


    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStockRequestDTO createDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var stockModel = createDTO.ToStockFromCreateDTO();

        await _stockRepository.CreateAsync(stockModel);

        return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel.ToStockDTO());
    }


    [HttpPut("{id:int}")]
    [LogSensitveActionAttrubite]
    public async Task<IActionResult> Put([FromRoute] int id, [FromBody] UpdateStockRequestDTO updateDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var stockModel = await _stockRepository.UpdateAsync(id, updateDTO);
        if (stockModel is null)
            return NotFound();

        return Ok(stockModel.ToStockDTO());
    }


    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var stockModel = await _stockRepository.DeleteAsync(id);
        if (stockModel is null)
            return NotFound();

        return NoContent();
    }
}