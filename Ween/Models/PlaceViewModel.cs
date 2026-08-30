namespace Ween.Models;

public class PlaceViewModel
{
    public int PlaceId { get; set; }

    public string Name { get; set; } = null!;

    public string? Tag { get; set; }

    public string Address { get; set; } = null!;

    public string? LocationUrl { get; set; }

    public string? Phone { get; set; }

    public string? Hours { get; set; }

    public string? Description { get; set; }

    public decimal Rating { get; set; }

    public int Capacity { get; set; }

    public bool IsActive { get; set; }

    public string CityName { get; set; } = null!;

    public string CitySlug { get; set; } = null!;

    public string CategoryName { get; set; } = null!;

    public string CategorySlug { get; set; } = null!;

    public string? CategoryIcon { get; set; }

    public List<PlacePhotoViewModel> Photos { get; set; } = new();

    public List<PlaceFieldValueViewModel> Fields { get; set; } = new();
}
