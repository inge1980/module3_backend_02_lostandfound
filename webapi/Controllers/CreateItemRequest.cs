using System.ComponentModel.DataAnnotations;

public class CreateItemRequest
{
    [Required]
    [MaxLength(80)]
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    [Required]
    public string FoundLocation { get; set; } = string.Empty;
}