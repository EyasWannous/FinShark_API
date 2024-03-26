
namespace api.options;

public class AttachmentsOptions
{
    public string AllowedExtensions { get; set; } = string.Empty;
    public int MaxSizeInMegaByte { get; set; }
    public bool EnableCompression { get; set; }
}