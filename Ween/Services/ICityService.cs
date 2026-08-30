using Ween.Models;

namespace Ween.Services;

public interface ICityService
{
    Task<List<CityCardViewModel>> GetCityCardsAsync();
}
