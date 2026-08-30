using Ween.Models;

namespace Ween.Services;

public interface IPlaceService
{
    Task<PlaceDetailsViewModel?> GetDetailsAsync(int placeId);
}
