namespace Ween.Services;

// Resolves a city slug -> palette tint color. Centralizes the "index in Name-ordered
// cities -> TintPalette" rule that was previously copy-pasted across services, and
// caches it so it isn't re-queried on every request.
public interface ICityTintResolver
{
    Task<IReadOnlyDictionary<string, string>> GetTintsAsync();

    Task<string> GetTintAsync(string citySlug);

    void Invalidate();
}
