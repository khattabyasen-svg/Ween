namespace Ween.Models;

public class AdminReservationsViewModel
{
    public int Total { get; set; }

    public int Confirmed { get; set; }

    public int Cancelled { get; set; }

    public string Filter { get; set; } = "all";

    public List<ReservationRowViewModel> Rows { get; set; } = new();
}
