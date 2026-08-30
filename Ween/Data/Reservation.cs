using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ween.Data;

[Index("PlaceId", Name = "IX_Reservations_Place")]
[Index("UserId", Name = "IX_Reservations_User")]
public partial class Reservation
{
    [Key]
    public int ReservationId { get; set; }

    public int PlaceId { get; set; }

    public int UserId { get; set; }

    public DateOnly ReservationDate { get; set; }

    public TimeOnly ReservationTime { get; set; }

    public int PartySize { get; set; }

    public int? Nights { get; set; }

    [StringLength(20)]
    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? CancelledAt { get; set; }

    [ForeignKey("PlaceId")]
    [InverseProperty("Reservations")]
    public virtual Place Place { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("Reservations")]
    public virtual ApplicationUser User { get; set; } = null!;
}
