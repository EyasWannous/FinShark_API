using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using api.Interfaces;
using api.Models;
using api.options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace api.Services;

public class TokenService : ITokenService
{
    // private readonly IConfiguration _config;
    private readonly SymmetricSecurityKey _symmetricSecurityKey;
    private readonly IOptions<SalafiJWTOptions> _options;

    public TokenService(IOptions<SalafiJWTOptions> options)
    {
        // _config = config;
        // _symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SigningKey"]!));
        _options = options;
        _symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.SigningKey));
    }

    public string CreateToken(AppUser appUser)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Email, appUser.Email!),
            new(JwtRegisteredClaimNames.GivenName, appUser.UserName!),
        };

        var cerds = new SigningCredentials(_symmetricSecurityKey, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddHours(2),
            SigningCredentials = cerds,
            Issuer = _options.Value.Issuer,
            Audience = _options.Value.Audience,
            // Issuer = _config["JWT:Issuer"],
            // Audience = _config["JWT:Audience"],
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        var accessToken = tokenHandler.WriteToken(securityToken);

        return accessToken;
    }
}