using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Ween.Data;

// Custom identity user with an int primary key (matches Places.CreatedByUserId
// and Reservations.UserId). Email, PhoneNumber, PasswordHash, UserName come from
// IdentityUser<int>; FullName and CreatedAt are app-specific.
public class ApplicationUser : IdentityUser<int>
{
    [StringLength(150)]
    public string FullName { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Place> Places { get; set; } = new List<Place>();

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
