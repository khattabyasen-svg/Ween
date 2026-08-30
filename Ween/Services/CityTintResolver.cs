using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Ween.Data;

namespace Ween.Services;

public class CityTintResolver : ICityTintResolver
{
    private const string CacheKey = "city-tints";

    private readonly WeenContext _db;
    private readonly IMemoryCache _cache;

    public CityTintResolver(WeenContext db, IMemoryCache cache)
    {
        _db = db;
        _cache = cache;
    }

    public async Task<IReadOnlyDictionary<string, string>> GetTintsAsync()
    {
        if (_cache.TryGetValue(CacheKey, out IReadOnlyDictionary<string, string>? cached) && cached is not null)
            return cached;

        var slugs = await _db.Cities
            .OrderBy(c => c.Name)
            .Select(c => c.Slug)
            .ToListAsync();

        var map = new Dictionary<string, string>(StringComparer.Ordinal);
        for (var i = 0; i < slugs.Count; i++)
            map[slugs[i]] = TintPalette.ForIndex(i);

        _cache.Set(CacheKey, (IReadOnlyDictionary<string, string>)map);
        return map;
    }

    public async Task<string> GetTintAsync(string citySlug)
    {
        var map = await GetTintsAsync();
        return map.TryGetValue(citySlug, out var tint) ? tint : TintPalette.ForIndex(0);
    }

    public void Invalidate() => _cache.Remove(CacheKey);
}
