namespace Ween.Services;

public record ReservationConfig(string Verb, string PartyLabel, bool ShowRoom);

public static class ReservationConfigs
{
    // Mirrors the prototype's RESERVATION_FIELDS map, keyed by category slug.
    public static ReservationConfig For(string? categorySlug) => categorySlug switch
    {
        "hotels" => new ReservationConfig("Book a room", "Guests", true),
        "transport" => new ReservationConfig("Reserve a seat", "Passengers", false),
        "coffee" => new ReservationConfig("Reserve a table", "Guests", false),
        "restaurants" => new ReservationConfig("Reserve a table", "Guests", false),
        _ => new ReservationConfig("Reserve", "Guests", false)
    };
}
