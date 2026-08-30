using System.ComponentModel.DataAnnotations;

namespace Ween.Models;

public class CreatePlaceInputModel
{
    [Required]
    public int CityId { get; set; }

    [Required]
    public int CategoryId { get; set; }

    [Required]
    [StringLength(200)]
    public string Name { get; set; } = null!;

    [Required]
    [StringLength(300)]
    public string Address { get; set; } = null!;

    [StringLength(30)]
    public string? Phone { get; set; }

    [StringLength(500)]
    public string? LocationUrl { get; set; }

    [StringLength(100)]
    public string? Hours { get; set; }

    [StringLength(100)]
    public string? Tag { get; set; }

    public string? Description { get; set; }

    [Range(1, 10000)]
    public int Capacity { get; set; } = 40;

    // Keyed by CategoryFieldId -> submitted value (from name="FieldValues[{id}]").
    public Dictionary<int, string> FieldValues { get; set; } = new();

    public List<IFormFile>? Photos { get; set; }
}
