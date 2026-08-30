using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Ween.Data;
using Ween.Models;

namespace Ween.Services;

public class ListingService : IListingService
{
    private readonly WeenContext _db;
    private readonly IMapper _mapper;
    private readonly ICityTintResolver _tints;

    public ListingService(WeenContext db, IMapper mapper, ICityTintResolver tints)
    {
        _db = db;
        _mapper = mapper;
        _tints = tints;
    }

    public async Task<ListingsPageViewModel?> GetListingsAsync(string citySlug, string categorySlug)
    {
        var city = await _db.Cities.FirstOrDefaultAsync(c => c.Slug == citySlug);
        if (city is null)
            return null;

        var category = await _db.Categories.FirstOrDefaultAsync(c => c.Slug == categorySlug);
        if (category is null)
            return null;

        var places = await _db.Places
            .Where(p => p.IsActive && p.City.Slug == citySlug && p.Category.Slug == categorySlug)
            .OrderByDescending(p => p.Rating)
            .ThenBy(p => p.Name)
            .ProjectTo<PlaceCardViewModel>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return new ListingsPageViewModel
        {
            CitySlug = city.Slug,
            CityName = city.Name,
            CityTint = await _tints.GetTintAsync(citySlug),
            CategorySlug = category.Slug,
            CategoryName = category.Name,
            CategoryIcon = category.Icon,
            Places = places
        };
    }
}
