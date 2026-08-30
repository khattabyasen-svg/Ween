namespace Ween.Models;

public class AdminFieldViewModel
{
    public int CategoryFieldId { get; set; }

    public string Label { get; set; } = null!;

    public string FieldType { get; set; } = "text";

    public string[] Options { get; set; } = Array.Empty<string>();
}
