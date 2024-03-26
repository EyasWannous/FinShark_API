using System.Security.Claims;

namespace api.Extensions;

public static class ClaimsExtensions
{
    public static string? GetUsername(this ClaimsPrincipal claimsPrincipal)
    {
        if (claimsPrincipal.Identity is not ClaimsIdentity principal)
            return null;

        var username = principal.FindFirst(ClaimTypes.GivenName)?.Value;
        // .FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname")?.Value;
        // .FindFirst(JwtRegisteredClaimNames.GivenName)?.Value;

        return username;

        // return claimsPrincipal.Claims
        //     .SingleOrDefault(claim
        //         => claim.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname")
        //     )?.Value!;
    }
}