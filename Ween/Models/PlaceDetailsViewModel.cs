namespace Ween.Models;

public class PlaceDetailsViewModel
{
    public PlaceViewModel Place { get; set; } = null!;

    public string CityTint { get; set; } = "";

    public string ReservationVerb { get; set; } = "Reserve";

    public string ReservationPartyLabel { get; set; } = "Guests";

    public bool ShowRoom { get; set; }
}
