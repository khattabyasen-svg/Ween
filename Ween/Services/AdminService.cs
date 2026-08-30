using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Ween.Data;
using Ween.Models;

namespace Ween.Services;

public class AdminService : IAdminService
{
    private static readonly string[] AllowedPhotoExtensions =
        { ".jpg", ".jpeg", ".png", ".webp", ".gif" };

    private const long MaxPhotoBytes = 5 * 1024 * 1024; // 5 MB per file

    private readonly WeenContext _db;
    private readonly IWebHostEnvironment _env;
    private readonly ICityTintResolver _tints;

    public AdminService(WeenContext db, IWebHostEnvironment env, ICityTintResolver tints)
    {
        _db = db;
        _env = env;
        _tints = tints;
    }

    public async Task<AdminDashboardViewModel> GetDashboardAsync()
    {
        var cities = await _db.Cities
            .OrderBy(c => c.Name)
            .Select(c => new AdminDashboardViewModel.CityChip(c.CityId, c.Name, c.LocalName, c.Slug))
            .ToListAsync();

        var categories = await _db.Categories
            .OrderBy(c => c.Name)
            .Select(c => new AdminDashboardViewModel.CategoryChip(c.CategoryId, c.Name, c.Icon, c.Slug))
            .ToListAsync();

        var initialFields = categories.Count > 0
            ? await GetCategoryFieldsAsync(categories[0].CategoryId)
            : new List<AdminFieldViewModel>();

        return new AdminDashboardViewModel
        {
            Cities = cities,
            Categories = categories,
            InitialFields = initialFields
        };
    }

    public async Task<List<AdminFieldViewModel>> GetCategoryFieldsAsync(int categoryId)
    {
        var fields = await _db.CategoryFields
            .Where(f => f.CategoryId == categoryId)
            .OrderBy(f => f.DisplayOrder)
            .Select(f => new { f.CategoryFieldId, f.Label, f.FieldType, f.Options })
            .ToListAsync();

        return fields.Select(f => new AdminFieldViewModel
        {
            CategoryFieldId = f.CategoryFieldId,
            Label = f.Label,
            FieldType = f.FieldType,
            Options = string.IsNullOrWhiteSpace(f.Options)
                ? Array.Empty<string>()
                : f.Options.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
        }).ToList();
    }

    public async Task AddCityAsync(string name, string? localName)
    {
        var slug = await UniqueSlugAsync(name, s => _db.Cities.AnyAsync(c => c.Slug == s));
        _db.Cities.Add(new City
        {
            Name = name.Trim(),
            LocalName = string.IsNullOrWhiteSpace(localName) ? null : localName.Trim(),
            Slug = slug,
            CreatedAt = DateTime.UtcNow
        });
        await _db.SaveChangesAsync();
        _tints.Invalidate(); // city set changed -> tint index map is stale
    }

    public async Task AddCategoryAsync(string name, string? icon)
    {
        var slug = await UniqueSlugAsync(name, s => _db.Categories.AnyAsync(c => c.Slug == s));
        _db.Categories.Add(new Category
        {
            Name = name.Trim(),
            Icon = string.IsNullOrWhiteSpace(icon) ? "📍" : icon.Trim(),
            Slug = slug,
            CreatedAt = DateTime.UtcNow
        });
        await _db.SaveChangesAsync();
    }

    public async Task<string?> CreatePlaceAsync(CreatePlaceInputModel model, int userId)
    {
        // Reject non-existent FKs before touching the DB (avoids an unhandled FK violation).
        var cityExists = await _db.Cities.AnyAsync(c => c.CityId == model.CityId);
        var categoryExists = await _db.Categories.AnyAsync(c => c.CategoryId == model.CategoryId);
        if (!cityExists || !categoryExists)
            return null;

        var place = new Place
        {
            CityId = model.CityId,
            CategoryId = model.CategoryId,
            Name = model.Name.Trim(),
            Address = model.Address.Trim(),
            Phone = Trimmed(model.Phone),
            LocationUrl = Trimmed(model.LocationUrl),
            Hours = Trimmed(model.Hours),
            Tag = Trimmed(model.Tag),
            Description = Trimmed(model.Description),
            Capacity = model.Capacity,
            Rating = 4.0m,
            IsActive = true,
            CreatedByUserId = userId,
            CreatedAt = DateTime.UtcNow
        };
        _db.Places.Add(place);
        await _db.SaveChangesAsync(); // assigns PlaceId

        // Dynamic (EAV) values — only fields that actually belong to this category.
        var validFieldIds = await _db.CategoryFields
            .Where(f => f.CategoryId == model.CategoryId)
            .Select(f => f.CategoryFieldId)
            .ToListAsync();

        foreach (var kv in model.FieldValues)
        {
            if (!validFieldIds.Contains(kv.Key) || string.IsNullOrWhiteSpace(kv.Value))
                continue;

            _db.PlaceFieldValues.Add(new PlaceFieldValue
            {
                PlaceId = place.PlaceId,
                CategoryFieldId = kv.Key,
                Value = kv.Value.Trim()
            });
        }

        await SavePhotosAsync(model.Photos, place.PlaceId);

        await _db.SaveChangesAsync();
        return place.Name;
    }

    private async Task SavePhotosAsync(List<IFormFile>? photos, int placeId)
    {
        if (photos is not { Count: > 0 })
            return;

        var uploadsDir = Path.Combine(_env.WebRootPath, "uploads");
        Directory.CreateDirectory(uploadsDir);

        var order = 0;
        foreach (var file in photos)
        {
            if (file.Length == 0 || file.Length > MaxPhotoBytes)
                continue;

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!AllowedPhotoExtensions.Contains(ext))
                continue;

            // Read bounded into memory and verify the bytes are actually an image of the
            // claimed type (magic-byte sniff) — an extension allowlist alone lets a
            // disguised HTML/script file be stored and served from our origin.
            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            var bytes = ms.ToArray();
            if (!IsValidImage(ext, bytes))
                continue;

            var fileName = $"{Guid.NewGuid():N}{ext}";
            await File.WriteAllBytesAsync(Path.Combine(uploadsDir, fileName), bytes);

            _db.PlacePhotos.Add(new PlacePhoto
            {
                PlaceId = placeId,
                PhotoUrl = $"/uploads/{fileName}",
                DisplayOrder = order++,
                UploadedAt = DateTime.UtcNow
            });
        }
    }

    private static bool IsValidImage(string ext, byte[] b)
    {
        bool StartsWith(params byte[] sig) =>
            b.Length >= sig.Length && sig.SequenceEqual(b.Take(sig.Length));

        return ext switch
        {
            ".jpg" or ".jpeg" => StartsWith(0xFF, 0xD8, 0xFF),
            ".png" => StartsWith(0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A),
            ".gif" => StartsWith(0x47, 0x49, 0x46, 0x38), // "GIF8"
            ".webp" => b.Length >= 12
                       && b.Take(4).SequenceEqual(new byte[] { 0x52, 0x49, 0x46, 0x46 })   // "RIFF"
                       && b.Skip(8).Take(4).SequenceEqual(new byte[] { 0x57, 0x45, 0x42, 0x50 }), // "WEBP"
            _ => false
        };
    }

    public async Task<AdminReservationsViewModel> GetReservationsAsync(string? filter)
    {
        filter = (filter ?? "all").ToLowerInvariant();
        if (filter != "confirmed" && filter != "cancelled")
            filter = "all";

        // One grouped query for all three counts instead of three table scans.
        var counts = await _db.Reservations
            .GroupBy(r => r.Status)
            .Select(g => new { Status = g.Key, Count = g.Count() })
            .ToListAsync();

        IQueryable<Reservation> query = _db.Reservations;
        if (filter == "confirmed")
            query = query.Where(r => r.Status == ReservationStatus.Confirmed);
        else if (filter == "cancelled")
            query = query.Where(r => r.Status == ReservationStatus.Cancelled);

        return new AdminReservationsViewModel
        {
            Total = counts.Sum(c => c.Count),
            Confirmed = counts.Where(c => c.Status == ReservationStatus.Confirmed).Sum(c => c.Count),
            Cancelled = counts.Where(c => c.Status == ReservationStatus.Cancelled).Sum(c => c.Count),
            Filter = filter,
            Rows = await ReservationRowProjection.ToRowsAsync(query, _tints)
        };
    }

    public async Task<bool> CancelReservationAsync(int reservationId)
    {
        var reservation = await _db.Reservations.FindAsync(reservationId);
        if (reservation is null)
            return false;

        if (reservation.Status != ReservationStatus.Cancelled)
        {
            reservation.Status = ReservationStatus.Cancelled;
            reservation.CancelledAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
        }
        return true;
    }

    private static string? Trimmed(string? s) => string.IsNullOrWhiteSpace(s) ? null : s.Trim();

    private static async Task<string> UniqueSlugAsync(string name, Func<string, Task<bool>> exists)
    {
        var baseSlug = Slugify(name);
        if (baseSlug.Length == 0)
            baseSlug = "item";

        var slug = baseSlug;
        var n = 2;
        while (await exists(slug))
            slug = $"{baseSlug}-{n++}";

        return slug;
    }

    private static string Slugify(string name)
    {
        var lowered = name.Trim().ToLowerInvariant();
        var slug = Regex.Replace(lowered, "[^a-z0-9]+", "-");
        return slug.Trim('-');
    }
}
