using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Ween.Data;
using Ween.Models;

namespace Ween.Services;

public class ReservationService : IReservationService
{
    private readonly WeenContext _db;
    private readonly ICityTintResolver _tints;

    public ReservationService(WeenContext db, ICityTintResolver tints)
    {
        _db = db;
        _tints = tints;
    }

    // Race-condition-safe daily capacity check. The locked SUM read, the capacity
    // check, and the INSERT all run inside ONE transaction on ONE connection.
    // UPDLOCK + HOLDLOCK take a key-range update lock on the (PlaceId, ReservationDate)
    // rows so a second concurrent request blocks on the SELECT until the first commits,
    // then re-reads the updated total — preventing the lost-update race and phantom inserts.
    public async Task<ReservationResult> CreateAsync(int userId, ReservationInputModel model)
    {
        if (model.ReservationDate < DateOnly.FromDateTime(DateTime.Today))
            return new ReservationResult(false, "You can't reserve a date in the past.", 0);

        await using var tx = await _db.Database.BeginTransactionAsync();

        // Only bookable if the place exists AND is active.
        var place = await _db.Places
            .Where(p => p.PlaceId == model.PlaceId && p.IsActive)
            .Select(p => new { p.Capacity, CategorySlug = p.Category.Slug })
            .FirstOrDefaultAsync();

        if (place is null)
        {
            await tx.RollbackAsync();
            return new ReservationResult(false, "This place is not available for booking.", 0);
        }

        // Only room-style categories (hotels) keep Nights; others ignore any posted value.
        var nights = ReservationConfigs.For(place.CategorySlug).ShowRoom ? model.Nights : null;

        // Raw SQL: EF Core's LINQ cannot express table locking hints, so this read
        // must be raw ADO on the SAME connection + transaction as the insert below.
        var conn = _db.Database.GetDbConnection();
        int existing;
        await using (var cmd = conn.CreateCommand())
        {
            cmd.Transaction = tx.GetDbTransaction();
            cmd.CommandText = $@"SELECT ISNULL(SUM(PartySize), 0)
FROM Reservations WITH (UPDLOCK, HOLDLOCK)
WHERE PlaceId = @placeId AND ReservationDate = @date AND Status = '{ReservationStatus.Confirmed}';";

            var pPlace = cmd.CreateParameter();
            pPlace.ParameterName = "@placeId";
            pPlace.DbType = DbType.Int32;
            pPlace.Value = model.PlaceId;
            cmd.Parameters.Add(pPlace);

            var pDate = cmd.CreateParameter();
            pDate.ParameterName = "@date";
            pDate.DbType = DbType.Date;
            pDate.Value = model.ReservationDate.ToDateTime(TimeOnly.MinValue);
            cmd.Parameters.Add(pDate);

            var scalar = await cmd.ExecuteScalarAsync();
            existing = Convert.ToInt32(scalar);
        }

        var remaining = place.Capacity - existing;
        if (model.PartySize > remaining)
        {
            await tx.RollbackAsync();
            var left = Math.Max(0, remaining);
            return new ReservationResult(false, $"Only {left} spots left for this date.", left);
        }

        _db.Reservations.Add(new Reservation
        {
            PlaceId = model.PlaceId,
            UserId = userId,
            ReservationDate = model.ReservationDate,
            ReservationTime = model.ReservationTime,
            PartySize = model.PartySize,
            Nights = nights,
            Status = ReservationStatus.Confirmed,
            CreatedAt = DateTime.UtcNow
        });

        await _db.SaveChangesAsync();   // participates in the same ambient transaction
        await tx.CommitAsync();

        return new ReservationResult(true, null, remaining - model.PartySize);
    }

    public Task<List<ReservationRowViewModel>> GetForUserAsync(int userId)
        => ReservationRowProjection.ToRowsAsync(_db.Reservations.Where(r => r.UserId == userId), _tints);

    public async Task<bool> CancelForUserAsync(int reservationId, int userId)
    {
        var reservation = await _db.Reservations
            .FirstOrDefaultAsync(r => r.ReservationId == reservationId && r.UserId == userId);

        if (reservation is null)
            return false; // not found or not owned by this user

        if (reservation.Status != ReservationStatus.Cancelled)
        {
            reservation.Status = ReservationStatus.Cancelled;
            reservation.CancelledAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
        }
        return true;
    }

    public async Task<(int capacity, int booked, int remaining)> GetDayUsageAsync(int placeId, DateOnly date)
    {
        var capacity = await _db.Places
            .Where(p => p.PlaceId == placeId)
            .Select(p => (int?)p.Capacity)
            .FirstOrDefaultAsync();

        if (capacity is null)
            return (0, 0, 0);

        var booked = await _db.Reservations
            .Where(r => r.PlaceId == placeId && r.ReservationDate == date && r.Status == ReservationStatus.Confirmed)
            .SumAsync(r => (int?)r.PartySize) ?? 0;

        return (capacity.Value, booked, Math.Max(0, capacity.Value - booked));
    }
}
