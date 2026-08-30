namespace Ween.Models;

public class CategoryViewModel
{
    public int CategoryId { get; set; }

    public string Slug { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Icon { get; set; }

    public int PlaceCount { get; set; }
}
