using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ween.Data;

[Index("CategoryId", Name = "IX_CategoryFields_Category")]
public partial class CategoryField
{
    [Key]
    public int CategoryFieldId { get; set; }

    public int CategoryId { get; set; }

    [StringLength(100)]
    public string Label { get; set; } = null!;

    [StringLength(20)]
    public string FieldType { get; set; } = null!;

    [StringLength(500)]
    public string? Options { get; set; }

    public int DisplayOrder { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("CategoryFields")]
    public virtual Category Category { get; set; } = null!;

    [InverseProperty("CategoryField")]
    public virtual ICollection<PlaceFieldValue> PlaceFieldValues { get; set; } = new List<PlaceFieldValue>();
}
