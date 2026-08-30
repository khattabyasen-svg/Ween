using System.Security.Claims;

namespace Ween.Extensions;

public static class ClaimsPrincipalExtensions
{
    // Returns the signed-in user's integer id, or null if the NameIdentifier claim
    // is missing/non-numeric (rather than throwing on int.Parse).
    public static int? GetUserId(this ClaimsPrincipal user)
    {
        var value = user.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(value, out var id) ? id : null;
    }
}
