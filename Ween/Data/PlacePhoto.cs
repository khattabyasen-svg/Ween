using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ween.Data;

[Index("PlaceId", Name = "IX_PlacePhotos_Place")]
public partial class PlacePhoto
{
    [Key]
    public int PhotoId { get; set; }

    public int PlaceId { get; set; }

    [StringLength(500)]
    public string PhotoUrl { get; set; } = null!;

    public int DisplayOrder { get; set; }

    public DateTime UploadedAt { get; set; }

    [ForeignKey("PlaceId")]
    [InverseProperty("PlacePhotos")]
    public virtual Place Place { get; set; } = null!;
}
