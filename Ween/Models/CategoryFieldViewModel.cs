namespace Ween.Models;

public class CategoryFieldViewModel
{
    public int CategoryFieldId { get; set; }

    public string Label { get; set; } = null!;

    public string FieldType { get; set; } = null!;

    public string? Options { get; set; }

    public int DisplayOrder { get; set; }
}
