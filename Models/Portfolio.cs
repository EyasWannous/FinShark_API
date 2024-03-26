using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models;

[Table("Portfolios")]
public class Portfolio
{
    public string AppUserId { get; set; } = string.Empty;
    public int StockId { get; set; }
    public required AppUser AppUser { get; set; }
    public required Stock Stock { get; set; }
}