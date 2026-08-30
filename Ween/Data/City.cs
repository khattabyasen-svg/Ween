using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ween.Data;

[Index("Slug", Name = "UQ__Cities__BC7B5FB62C359231", IsUnique = true)]
public partial class City
{
    [Key]
    public int CityId { get; set; }

    [StringLength(100)]
    public string Name { get; set; } = null!;

    [StringLength(100)]
    public string? LocalName { get; set; }

    [StringLength(120)]
    public string Slug { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    [InverseProperty("City")]
    public virtual ICollection<Place> Places { get; set; } = new List<Place>();
}
