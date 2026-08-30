using System.ComponentModel.DataAnnotations;

namespace Ween.Models;

public class AddCategoryInputModel
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [StringLength(20)]
    public string? Icon { get; set; }
}
