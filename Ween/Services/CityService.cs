using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Ween.Data;
using Ween.Models;

namespace Ween.Services;

public class CityService : ICityService
{
    private readonly WeenContext _db;
    private readonly IMapper _mapper;
    private readonly ICityTintResolver _tints;

    public CityService(WeenContext db, IMapper mapper, ICityTintResolver tints)
    {
        _db = db;
        _mapper = mapper;
        _tints = tints;
    }

    public async Task<List<CityCardViewModel>> GetCityCardsAsync()
    {
        var cards = await _db.Cities
            .OrderBy(c => c.Name)
            .ProjectTo<CityCardViewModel>(_mapper.ConfigurationProvider)
            .ToListAsync();

        var tints = await _tints.GetTintsAsync();
        foreach (var card in cards)
            card.Tint = tints.TryGetValue(card.Slug, out var tint) ? tint : "";

        return cards;
    }
}
