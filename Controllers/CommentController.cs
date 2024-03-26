using api.DTOs.Comment;
using api.Extensions;
using api.Interfaces;
using api.Mappers;
using api.Models;
using api.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CommentController(ICommentRepository commentRepository, IStockRepository stockRepository, UserManager<AppUser> userManager) : ControllerBase
{
    private readonly ICommentRepository _commentRepository = commentRepository;
    private readonly IStockRepository _stockRepository = stockRepository;
    private readonly UserManager<AppUser> _userManager = userManager;


    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var comments = await _commentRepository.GetAllAsync();
        var commentsDTO = comments.Select(comment => comment.ToCommentDTO());

        return Ok(commentsDTO);
    }


    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var comment = await _commentRepository.GetByIdAsync(id);
        if (comment is null)
            return NotFound();

        return Ok(comment.ToCommentDTO());
    }


    [HttpPost("{stockId:int}")]
    public async Task<IActionResult> Create([FromRoute] int stockId, [FromBody] CreateCommentRequestDTO commentDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!await _stockRepository.StockExists(stockId))
            return BadRequest("Stock Does Not Exist");

        var username = User.GetUsername();
        if (username is null)
            return BadRequest("User Not Found");

        var appUser = await _userManager.FindByNameAsync(username);

        if (appUser is null)
            return Unauthorized();

        var commentModel = commentDTO.ToCommentFromCreateDTO(stockId, appUser);

        commentModel.AppUserId = appUser.Id.ToString();

        await _commentRepository.CreateAsync(commentModel);

        return CreatedAtAction(nameof(GetById), new { id = commentModel.Id }, commentModel.ToCommentDTO());
    }


    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, UpdateCommentRequestDTO commetnDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var username = User.GetUsername();
        if (username is null)
            return BadRequest("User Not Found");

        var appUser = await _userManager.FindByNameAsync(username);

        if (appUser is null)
            return Unauthorized();

        var commentModel = await _commentRepository.UpdateAsync(id, commetnDTO.ToCommentFromUpdate(id, appUser));
        if (commentModel is null)
            return NotFound();

        return Ok(commentModel.ToCommentDTO());
    }


    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var commentModel = await _commentRepository.DeleteAsync(id);
        if (commentModel is null)
            return NotFound();

        return NoContent();
    }
}