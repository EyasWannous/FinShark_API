using api.DTOs.Account;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class AccountController(UserManager<AppUser> userManager,
    ITokenService tokenService, SignInManager<AppUser> signInManager) : ControllerBase
{
    private readonly UserManager<AppUser> _userManager = userManager;
    private readonly ITokenService _tokenService = tokenService;
    private readonly SignInManager<AppUser> _signInManager = signInManager;


    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterdDTO registerdDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var appUser = new AppUser
        {
            UserName = registerdDTO.UserName,
            Email = registerdDTO.Email,
        };

        try
        {
            var createUser = await _userManager.CreateAsync(appUser, registerdDTO.Password!);

            if (!createUser.Succeeded)
                return StatusCode(500, createUser.Errors);

            var roleResult = await _userManager.AddToRoleAsync(appUser, "User");

            if (!roleResult.Succeeded)
                return StatusCode(500, roleResult.Errors);

            var newUser = new NewUserCreatedDTO
            {
                UserName = appUser.UserName!,
                Email = appUser.Email!,
                Token = _tokenService.CreateToken(appUser),
            };

            return Ok(newUser);
        }
        catch (Exception error)
        {
            return StatusCode(500, error);
        }
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDTO loginDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == loginDTO.UserName);
        if (user is null)
            return Unauthorized("Invalid UserName!");

        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDTO.Password, false);

        if (!result.Succeeded)
            return Unauthorized("UserName not found and/or Password incorrect");

        return Ok(
            new NewUserCreatedDTO
            {
                UserName = user.UserName!,
                Email = user.Email!,
                Token = _tokenService.CreateToken(user),
            }
        );
    }
}