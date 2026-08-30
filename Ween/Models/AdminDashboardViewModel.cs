namespace Ween.Models;

public class AdminDashboardViewModel
{
    public List<CityChip> Cities { get; set; } = new();

    public List<CategoryChip> Categories { get; set; } = new();

    public List<AdminFieldViewModel> InitialFields { get; set; } = new();

    public record CityChip(int CityId, string Name, string? LocalName, string Slug);

    public record CategoryChip(int CategoryId, string Name, string? Icon, string Slug);
}
