using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Ween.Data;

public partial class WeenContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
{
    public WeenContext(DbContextOptions<WeenContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<CategoryField> CategoryFields { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Place> Places { get; set; }

    public virtual DbSet<PlaceFieldValue> PlaceFieldValues { get; set; }

    public virtual DbSet<PlacePhoto> PlacePhotos { get; set; }

    public virtual DbSet<Reservation> Reservations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Categori__19093A0BA4D11AEB");
                
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
        });

        modelBuilder.Entity<CategoryField>(entity =>
        {
            entity.HasKey(e => e.CategoryFieldId).HasName("PK__Category__213B041276F586DD");

            entity.Property(e => e.FieldType).HasDefaultValue("text");

            entity.HasOne(d => d.Category).WithMany(p => p.CategoryFields).HasConstraintName("FK_CategoryFields_Categories");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.CityId).HasName("PK__Cities__F2D21B76630E1DB8");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
        });

        modelBuilder.Entity<Place>(entity =>
        {
            entity.HasKey(e => e.PlaceId).HasName("PK__Places__D5222B6E06024839");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Capacity).HasDefaultValue(40);

            entity.HasOne(d => d.Category).WithMany(p => p.Places)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Places_Categories");

            entity.HasOne(d => d.City).WithMany(p => p.Places)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Places_Cities");

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.Places)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Places_Users");
        });

        modelBuilder.Entity<PlaceFieldValue>(entity =>
        {
            entity.HasKey(e => e.PlaceFieldValueId).HasName("PK__PlaceFie__8A47BA3A49C1F373");

            entity.HasOne(d => d.CategoryField).WithMany(p => p.PlaceFieldValues)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PlaceFieldValues_CategoryFields");

            entity.HasOne(d => d.Place).WithMany(p => p.PlaceFieldValues).HasConstraintName("FK_PlaceFieldValues_Places");
        });

        modelBuilder.Entity<PlacePhoto>(entity =>
        {
            entity.HasKey(e => e.PhotoId).HasName("PK__PlacePho__21B7B5E28CC0AFDD");

            entity.Property(e => e.UploadedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Place).WithMany(p => p.PlacePhotos).HasConstraintName("FK_PlacePhotos_Places");
        });

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasKey(e => e.ReservationId).HasName("PK__Reservat__B7EE5F247E1C2B36");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Status).HasDefaultValue("Confirmed");

            entity.HasOne(d => d.Place).WithMany(p => p.Reservations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reservations_Places");

            entity.HasOne(d => d.User).WithMany(p => p.Reservations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reservations_Users");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
