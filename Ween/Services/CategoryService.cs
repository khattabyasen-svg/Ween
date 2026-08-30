using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Ween.Data;
using Ween.Models;

namespace Ween.Services;

public class CategoryService : ICategoryService
{
    private readonly WeenContext _db;
    private readonly IMapper _mapper;

    public CategoryService(WeenContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<CategoriesPageViewModel?> GetCategoriesForCityAsync(string citySlug)
    {
        var city = await _db.Cities
            .FirstOrDefaultAsync(c => c.Slug == citySlug);

        if (city is null)
            return null;

        var categories = await _db.Categories
            .OrderBy(c => c.Name)
            .ProjectTo<CategoryViewModel>(_mapper.ConfigurationProvider, new { citySlug })
            .ToListAsync();

        return new CategoriesPageViewModel
        {
            CitySlug = city.Slug,
            CityName = city.Name,
            CityLocalName = city.LocalName ?? "",
            Categories = categories
        };
    }
}
