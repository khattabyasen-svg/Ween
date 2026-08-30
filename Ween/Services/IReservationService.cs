using Ween.Models;

namespace Ween.Services;

public interface IReservationService
{
    Task<ReservationResult> CreateAsync(int userId, ReservationInputModel model);

    Task<(int capacity, int booked, int remaining)> GetDayUsageAsync(int placeId, DateOnly date);

    Task<List<ReservationRowViewModel>> GetForUserAsync(int userId);

    Task<bool> CancelForUserAsync(int reservationId, int userId);
}
