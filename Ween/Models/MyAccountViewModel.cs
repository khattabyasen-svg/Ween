namespace Ween.Models;

public class MyAccountViewModel
{
    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PhoneNumber { get; set; } = "";

    public string Role { get; set; } = "Customer";

    public DateTime MemberSince { get; set; }

    public int ConfirmedCount { get; set; }

    public List<ReservationRowViewModel> Reservations { get; set; } = new();
}
