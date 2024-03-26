using Microsoft.AspNetCore.Identity;

namespace api.Models;
public class AppUser : IdentityUser
{
    public List<Portfolio> Portfolios { get; set; } = [];

    public List<Comment> Comments { get; set; } = [];
}