using AutoMapper;
using Ween.Data;
using Ween.Models;

namespace Ween.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<City, CityCardViewModel>()
            .ForMember(d => d.Count, o => o.MapFrom(s => s.Places.Count(p => p.IsActive)))
            .ForMember(d => d.LocalName, o => o.MapFrom(s => s.LocalName ?? ""))
            .ForMember(d => d.Tint, o => o.Ignore()); // assigned by index after query

        // citySlug is a ProjectTo parameter, substituted per-query (see CategoryService).
        // Its only consumer is the category page, which always supplies it.
        string? citySlug = null;
        CreateMap<Category, CategoryViewModel>()
            .ForMember(d => d.PlaceCount,
                o => o.MapFrom(s => s.Places.Count(p => p.City.Slug == citySlug && p.IsActive)));

        CreateMap<CategoryField, CategoryFieldViewModel>();

        CreateMap<PlacePhoto, PlacePhotoViewModel>();

        CreateMap<Place, PlaceCardViewModel>()
            .ForMember(d => d.CategoryIcon, o => o.MapFrom(s => s.Category.Icon))
            .ForMember(d => d.CoverPhotoUrl, o => o.MapFrom(s =>
                s.PlacePhotos.OrderBy(ph => ph.DisplayOrder).Select(ph => ph.PhotoUrl).FirstOrDefault()));

        CreateMap<PlaceFieldValue, PlaceFieldValueViewModel>()
            .ForMember(d => d.Label, o => o.MapFrom(s => s.CategoryField.Label));

        CreateMap<Place, PlaceViewModel>()
            .ForMember(d => d.CityName, o => o.MapFrom(s => s.City.Name))
            .ForMember(d => d.CitySlug, o => o.MapFrom(s => s.City.Slug))
            .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.Category.Name))
            .ForMember(d => d.CategorySlug, o => o.MapFrom(s => s.Category.Slug))
            .ForMember(d => d.CategoryIcon, o => o.MapFrom(s => s.Category.Icon))
            .ForMember(d => d.Photos, o => o.MapFrom(s => s.PlacePhotos))
            .ForMember(d => d.Fields, o => o.MapFrom(s => s.PlaceFieldValues));

        CreateMap<Reservation, ReservationViewModel>()
            .ForMember(d => d.PlaceName, o => o.MapFrom(s => s.Place.Name))
            .ForMember(d => d.UserFullName, o => o.MapFrom(s => s.User.FullName));

        CreateMap<ApplicationUser, UserViewModel>()
            .ForMember(d => d.UserId, o => o.MapFrom(s => s.Id))
            .ForMember(d => d.Role, o => o.Ignore()); // roles live in Identity's role tables, not a scalar column
    }
}
