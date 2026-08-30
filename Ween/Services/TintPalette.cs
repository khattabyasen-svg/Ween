namespace Ween.Services;

public static class TintPalette
{
    private static readonly string[] Colors =
    {
        "#2D6E7E", "#A8452F", "#3D5A47", "#1D5C73",
        "#7A4A2B", "#8B3A3A", "#556B4E", "#6B4E3D"
    };

    public static string ForIndex(int i) =>
        Colors[((i % Colors.Length) + Colors.Length) % Colors.Length];
}
