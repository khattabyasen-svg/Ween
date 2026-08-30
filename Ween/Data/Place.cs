using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ween.Data;

[Index("CategoryId", Name = "IX_Places_Category")]
[Index("CityId", "CategoryId", Name = "IX_Places_City_Category")]
public partial class Place
{
    [Key]
    public int PlaceId { get; set; }

    public int CityId { get; set; }

    public int CategoryId { get; set; }

    public int? CreatedByUserId { get; set; }

    [StringLength(200)]
    public string Name { get; set; } = null!;

    [StringLength(100)]
    public string? Tag { get; set; }

    [StringLength(300)]
    public string Address { get; set; } = null!;

    [StringLength(500)]
    public string? LocationUrl { get; set; }

    [StringLength(30)]
    public string? Phone { get; set; }

    [StringLength(100)]
    public string? Hours { get; set; }

    public string? Description { get; set; }

    [Column(TypeName = "decimal(2, 1)")]
    public decimal Rating { get; set; }

    public int Capacity { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("Places")]
    public virtual Category Category { get; set; } = null!;

    [ForeignKey("CityId")]
    [InverseProperty("Places")]
    public virtual City City { get; set; } = null!;

    [ForeignKey("CreatedByUserId")]
    [InverseProperty("Places")]
    public virtual ApplicationUser? CreatedByUser { get; set; }

    [InverseProperty("Place")]
    public virtual ICollection<PlaceFieldValue> PlaceFieldValues { get; set; } = new List<PlaceFieldValue>();

    [InverseProperty("Place")]
    public virtual ICollection<PlacePhoto> PlacePhotos { get; set; } = new List<PlacePhoto>();

    [InverseProperty("Place")]
    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
