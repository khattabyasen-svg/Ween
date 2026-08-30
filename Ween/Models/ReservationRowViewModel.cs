namespace Ween.Models;

public class ReservationRowViewModel
{
    public int ReservationId { get; set; }

    public string PlaceName { get; set; } = null!;

    public string? CategoryIcon { get; set; }

    public string CityName { get; set; } = null!;

    public string CityTint { get; set; } = "";

    public DateOnly ReservationDate { get; set; }

    public TimeOnly ReservationTime { get; set; }

    public int PartySize { get; set; }

    public int? Nights { get; set; }

    public string CustomerName { get; set; } = null!;

    public string Status { get; set; } = null!;
}
