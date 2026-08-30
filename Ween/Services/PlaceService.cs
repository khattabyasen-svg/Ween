using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Ween.Data;
using Ween.Models;

namespace Ween.Services;

public class PlaceService : IPlaceService
{
    private readonly WeenContext _db;
    private readonly IMapper _mapper;
    private readonly ICityTintResolver _tints;

    public PlaceService(WeenContext db, IMapper mapper, ICityTintResolver tints)
    {
        _db = db;
        _mapper = mapper;
        _tints = tints;
    }

    public async Task<PlaceDetailsViewModel?> GetDetailsAsync(int placeId)
    {
        var place = await _db.Places
            .Where(p => p.PlaceId == placeId && p.IsActive)
            .ProjectTo<PlaceViewModel>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        if (place is null)
            return null;

        var config = ReservationConfigs.For(place.CategorySlug);

        return new PlaceDetailsViewModel
        {
            Place = place,
            CityTint = await _tints.GetTintAsync(place.CitySlug),
            ReservationVerb = config.Verb,
            ReservationPartyLabel = config.PartyLabel,
            ShowRoom = config.ShowRoom
        };
    }
}
