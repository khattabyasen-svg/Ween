namespace Ween.Models;

public class ReservationViewModel
{
    public int ReservationId { get; set; }

    public string PlaceName { get; set; } = null!;

    public DateOnly ReservationDate { get; set; }

    public TimeOnly ReservationTime { get; set; }

    public int PartySize { get; set; }

    public int? Nights { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? CancelledAt { get; set; }

    public string UserFullName { get; set; } = null!;
}
