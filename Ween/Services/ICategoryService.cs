using Ween.Models;

namespace Ween.Services;

public interface ICategoryService
{
    Task<CategoriesPageViewModel?> GetCategoriesForCityAsync(string citySlug);
}
