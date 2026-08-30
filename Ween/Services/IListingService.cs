using Ween.Models;

namespace Ween.Services;

public interface IListingService
{
    Task<ListingsPageViewModel?> GetListingsAsync(string citySlug, string categorySlug);
}
