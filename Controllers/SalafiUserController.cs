using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using api.options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace api.Controllers;

//this conroller from https://www.youtube.com/watch?v=H781NNEfSD0&list=PLsV97AQt78NQ8E7cEqovH0zLYRJgJahGh&index=12
//JWT without useing packages like "Identity.entity" or "Identity.user" smth like that 
//salafi on fire 

// [ApiController]
// [Route("user/[salafiuserController]")]
public class SalafiUserController(SalafiJWTOptions jwtOptions) : ControllerBase
{
    // [HttpPost("auth")]
    public ActionResult<string> AuthenticateUser(SalafiAuthenticationRequset authenticationRequset)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = jwtOptions.Issuer,
            Audience = jwtOptions.Audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtOptions?.SigningKey!)
                ),
                SecurityAlgorithms.HmacSha256
            ),

            Subject = new ClaimsIdentity(new Claim[] {
                new (ClaimTypes.NameIdentifier,authenticationRequset.UserName),
                new (ClaimTypes.Email, "eyas@wannous.com"),
            }),
        };
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        var accessToken = tokenHandler.WriteToken(securityToken);
        return Ok(accessToken);
    }
}
