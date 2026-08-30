using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ween.Data;

[Index("PlaceId", "CategoryFieldId", Name = "UQ_PlaceFieldValues", IsUnique = true)]
public partial class PlaceFieldValue
{
    [Key]
    public int PlaceFieldValueId { get; set; }

    public int PlaceId { get; set; }

    public int CategoryFieldId { get; set; }

    [StringLength(300)]
    public string? Value { get; set; }

    [ForeignKey("CategoryFieldId")]
    [InverseProperty("PlaceFieldValues")]
    public virtual CategoryField CategoryField { get; set; } = null!;

    [ForeignKey("PlaceId")]
    [InverseProperty("PlaceFieldValues")]
    public virtual Place Place { get; set; } = null!;
}
