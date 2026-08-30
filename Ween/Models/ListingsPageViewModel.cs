namespace Ween.Models;

public class ListingsPageViewModel
{
    public string CitySlug { get; set; } = null!;

    public string CityName { get; set; } = null!;

    public string CityTint { get; set; } = "";

    public string CategorySlug { get; set; } = null!;

    public string CategoryName { get; set; } = null!;

    public string? CategoryIcon { get; set; }

    public List<PlaceCardViewModel> Places { get; set; } = new();
}
