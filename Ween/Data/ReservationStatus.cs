namespace Ween.Data;

// Single source of truth for the reservation Status column values, referenced by
// both the EF/LINQ paths and the raw capacity SQL so they can never drift apart.
public static class ReservationStatus
{
    public const string Confirmed = "Confirmed";
    public const string Cancelled = "Cancelled";
}
