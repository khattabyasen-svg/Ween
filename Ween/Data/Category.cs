using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ween.Data;

[Index("Slug", Name = "UQ__Categori__BC7B5FB6F7AFA51D", IsUnique = true)]
public partial class Category
{
    [Key]
    public int CategoryId { get; set; }

    [StringLength(100)]
    public string Name { get; set; } = null!;

    [StringLength(120)]
    public string Slug { get; set; } = null!;

    [StringLength(20)]
    public string? Icon { get; set; }

    public DateTime CreatedAt { get; set; }

    [InverseProperty("Category")]
    public virtual ICollection<CategoryField> CategoryFields { get; set; } = new List<CategoryField>();

    [InverseProperty("Category")]
    public virtual ICollection<Place> Places { get; set; } = new List<Place>();
}
