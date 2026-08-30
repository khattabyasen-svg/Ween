using Microsoft.EntityFrameworkCore;
using Ween.Data;
using Ween.Models;

namespace Ween.Services;

// Shared projection of reservations -> ReservationRowViewModel used by both the admin
// "all reservations" list and the customer "my reservations" list, so the two never drift.
public static class ReservationRowProjection
{
    public static async Task<List<ReservationRowViewModel>> ToRowsAsync(
        IQueryable<Reservation> query, ICityTintResolver tints)
    {
        var rows = await query
            .OrderByDescending(r => r.ReservationDate)
            .ThenByDescending(r => r.ReservationTime)
            .Select(r => new
            {
                r.ReservationId,
                PlaceName = r.Place.Name,
                CategoryIcon = r.Place.Category.Icon,
                CityName = r.Place.City.Name,
                CitySlug = r.Place.City.Slug,
                r.ReservationDate,
                r.ReservationTime,
                r.PartySize,
                r.Nights,
                CustomerName = r.User.FullName,
                r.Status
            })
            .ToListAsync();

        var tintMap = await tints.GetTintsAsync();

        return rows.Select(r => new ReservationRowViewModel
        {
            ReservationId = r.ReservationId,
            PlaceName = r.PlaceName,
            CategoryIcon = r.CategoryIcon,
            CityName = r.CityName,
            CityTint = tintMap.TryGetValue(r.CitySlug, out var tint) ? tint : "",
            ReservationDate = r.ReservationDate,
            ReservationTime = r.ReservationTime,
            PartySize = r.PartySize,
            Nights = r.Nights,
            CustomerName = r.CustomerName,
            Status = r.Status
        }).ToList();
    }
}
