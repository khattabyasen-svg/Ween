using Ween.Models;

namespace Ween.Services;

public interface IAdminService
{
    Task<AdminDashboardViewModel> GetDashboardAsync();

    Task<List<AdminFieldViewModel>> GetCategoryFieldsAsync(int categoryId);

    Task AddCityAsync(string name, string? localName);

    Task AddCategoryAsync(string name, string? icon);

    Task<string?> CreatePlaceAsync(CreatePlaceInputModel model, int userId);

    Task<AdminReservationsViewModel> GetReservationsAsync(string? filter);

    Task<bool> CancelReservationAsync(int reservationId);
}
