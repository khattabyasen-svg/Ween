# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Overview

Ween is an ASP.NET Core MVC web application (`.NET 10`) for browsing and reserving places (restaurants, venues, etc.) organized by city and category. It uses Entity Framework Core with SQL Server. The solution (`Ween.slnx`) contains a single project, `Ween/Ween.csproj`.

## Commands

Run all commands from the repository root.

- Build: `dotnet build`
- Run (default profile): `dotnet run --project Ween`
- Run with HTTPS: `dotnet run --project Ween --launch-profile https`
- Restore packages: `dotnet restore`

The app listens on `http://localhost:5191` (and `https://localhost:7190` with the `https` profile). There is no test project.

### Database

The app connects to SQL Server via the `WeenConnection` connection string in `Ween/appsettings.json` (defaults to `Server=localhost;Database=mishwark;Trusted_Connection=True`). A running SQL Server instance with the `mishwark` database is required.

The business tables were originally **database-first**: the entity classes (`Category`, `City`, `Place`, `Reservation`, etc.) were scaffolded from the existing schema, and their config lives in `WeenContext.OnModelCreating` (default values, FK delete behaviors, DB-generated PK constraint names). Those tables are still owned by the DB — regenerate their models with:

```
dotnet ef dbcontext scaffold "Name=ConnectionStrings:WeenConnection" Microsoft.EntityFrameworkCore.SqlServer --context WeenContext --context-dir Data --output-dir Data
```

**However, EF Core migrations are now in use** (`Ween/Migrations/`) for the ASP.NET Core Identity layer. The first migration (`Baseline`) is an intentional **no-op** that records the pre-existing schema in the model snapshot so migrations don't try to recreate the business tables; `AddIdentity` creates the `AspNet*` tables and repoints the `Places`/`Reservations` FKs to `AspNetUsers`. Apply migrations with `dotnet ef database update`. When adding a migration that must not touch the scaffolded business tables, review the generated `Up()` before applying.

### Authentication

Full **ASP.NET Core Identity** with cookie auth and integer keys. `ApplicationUser : IdentityUser<int>` (`Ween/Data/ApplicationUser.cs`) replaces the old custom `User` entity; `WeenContext` derives from `IdentityDbContext<ApplicationUser, IdentityRole<int>, int>` (so `OnModelCreating` must call `base.OnModelCreating` first). Two roles — **Admin** and **Customer** — plus a dev admin (`admin@ween.local` / `Admin#123`) are seeded at startup by `IdentitySeeder.SeedAsync` (`Program.cs`). `AccountController` uses `UserManager`/`SignInManager`; new registrations get the `Customer` role. Protect admin-only endpoints with `[Authorize(Roles = "Admin")]` and logged-in-only endpoints with `[Authorize]`.

## Architecture

Standard ASP.NET Core MVC over a **service layer**, wired up in `Ween/Program.cs` (`WeenContext` registered as a scoped DbContext; AutoMapper; the services below).

- `Ween/Controllers/` — thin MVC controllers. They inject **service interfaces** (`ICityService`, `ICategoryService`, `IListingService`, `IPlaceService`, `IReservationService`, `IAdminService`), not `WeenContext` directly; controllers hold no data-access logic.
- `Ween/Services/` — the data-access + business layer. Each service takes `WeenContext` (+ `IMapper` where it projects). Notable: `ReservationService.CreateAsync` holds the race-safe daily-capacity check (transaction + raw `UPDLOCK/HOLDLOCK` SUM); `ICityTintResolver` centralizes+caches the city→tint palette lookup; `ReservationRowProjection` is the shared reservation-row projection; `ReservationStatus` holds the `Confirmed`/`Cancelled` constants.
- `Ween/Mapping/MappingProfile.cs` — AutoMapper entity→view-model maps (queried via `ProjectTo`); validated at startup with `AssertConfigurationIsValid`.
- `Ween/Views/` — Razor views, organized per controller plus `Shared/`.
- `Ween/Data/` — EF Core entities, `WeenContext`, `IdentitySeeder`, `ReservationStatus`.
- `Ween/Models/` — view models and form input models.

### Data model

`WeenContext.cs` (`Ween/Data/`) is the central data layer. Key relationships:

- **Place** is the core entity, belonging to a `City` and a `Category`, optionally created by a `User`.
- **Category** defines a type of place and owns a set of **CategoryField** definitions (dynamic, custom fields — label, field type, options, display order).
- **PlaceFieldValue** is the value of a `CategoryField` for a specific `Place` — this is an EAV-style pattern letting each category define its own attributes rather than adding columns.
- **PlacePhoto** holds images for a `Place`; **Reservation** links a `User` to a `Place` (status defaults to `Confirmed`).
- **User** has a `Role` defaulting to `Customer`.

Entity configuration lives partly in data-annotation attributes on the entity classes and partly in `OnModelCreating` in `WeenContext.cs` (default values such as `sysutcdatetime()` timestamps, `IsActive`/`Status`/`Role` defaults, and FK delete behaviors). Because classes are `partial` and `OnModelCreating` calls `OnModelCreatingPartial`, add custom, non-scaffolded configuration in a separate partial class file so it survives re-scaffolding.
