using System.ComponentModel.DataAnnotations;

namespace Ween.Models;

public class AddCityInputModel
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [StringLength(100)]
    public string? LocalName { get; set; }
}
