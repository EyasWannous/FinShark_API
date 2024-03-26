
namespace api.options;

public class SalafiJWTOptions
{
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int Lifetime { get; set; }
    public string SigningKey { get; set; } = string.Empty;
}