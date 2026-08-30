namespace Ween.Models;

public class CityCardViewModel
{
    public string Slug { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string LocalName { get; set; } = "";

    public int Count { get; set; }

    public string Tint { get; set; } = "";
}
