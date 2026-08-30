namespace Ween.Models;

public class CategoriesPageViewModel
{
    public string CitySlug { get; set; } = null!;

    public string CityName { get; set; } = null!;

    public string CityLocalName { get; set; } = "";

    public List<CategoryViewModel> Categories { get; set; } = new();
}
