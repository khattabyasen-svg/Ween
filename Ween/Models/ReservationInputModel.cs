using System.ComponentModel.DataAnnotations;

namespace Ween.Models;

public class ReservationInputModel
{
    [Required]
    public int PlaceId { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateOnly ReservationDate { get; set; }

    [Required]
    [DataType(DataType.Time)]
    public TimeOnly ReservationTime { get; set; }

    [Required]
    [Range(1, 50)]
    public int PartySize { get; set; }

    [Range(1, 60)]
    public int? Nights { get; set; }
}
