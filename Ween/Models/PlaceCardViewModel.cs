namespace Ween.Models;

// Slim projection for the listings grid — scalars + a single cover photo, so the
// grid query doesn't drag each place's full Photos and Fields (EAV) collections.
public class PlaceCardViewModel
{
    public int PlaceId { get; set; }

    public string Name { get; set; } = null!;

    public string? Tag { get; set; }

    public string Address { get; set; } = null!;

    public decimal Rating { get; set; }

    public string? CategoryIcon { get; set; }

    public string? CoverPhotoUrl { get; set; }
}
