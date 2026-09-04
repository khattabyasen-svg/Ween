# Ween

An ASP.NET Core MVC web application for browsing and reserving places (restaurants, venues, etc.) organized by city and category.

## Prerequisites

| Requirement | Version |
|---|---|
| .NET SDK | 10.0 |
| SQL Server | 2019+ (or Express / LocalDB) |
| EF Core CLI | included with .NET SDK |

## Getting started

### 1. Clone the repository

```bash
git clone https://github.com/<your-username>/Ween.git
cd Ween
```

### 2. Configure the database connection

Open `Ween/appsettings.json` and update the connection string to point at your SQL Server instance:

```json
"ConnectionStrings": {
  "WeenConnection": "Server=localhost;Database=mishwark;Trusted_Connection=True;TrustServerCertificate=True"
}
```

> If you are using SQL Server Authentication instead of Windows Authentication, replace `Trusted_Connection=True` with `User Id=<user>;Password=<pass>`.

### 3. Restore packages

```bash
dotnet restore
```

### 4. Apply migrations

The project uses EF Core migrations for the Identity layer. Run the following to create the `AspNet*` tables and apply any schema changes:

```bash
dotnet ef database update --project Ween
```

> Make sure the `mishwark` database already exists on your SQL Server instance before running this command. The business tables (Cities, Categories, Places, Reservations, etc.) are expected to be present in the database from the original schema.

### 5. Run the application

```bash
# HTTP (http://localhost:5191)
dotnet run --project Ween

# HTTPS (https://localhost:7190)
dotnet run --project Ween --launch-profile https
```

Open your browser and navigate to `http://localhost:5191`.

## Default admin account

A dev admin account is seeded automatically on first run:

| Field | Value |
|---|---|
| Email | `admin@ween.local` |
| Password | `Admin#123` |

## Project structure

```
Ween/
├── Controllers/        # Thin MVC controllers (no direct DB access)
├── Data/               # EF Core entities, WeenContext, IdentitySeeder
├── Mapping/            # AutoMapper profiles (MappingProfile.cs)
├── Migrations/         # EF Core migrations
├── Models/             # View models and form input models
├── Services/           # Business + data-access layer (interfaces + implementations)
├── Views/              # Razor views
├── Program.cs          # App startup, DI registration, seeding
└── appsettings.json    # Connection strings and logging config
```

## Key features

- Browse places by city and category
- Dynamic per-category custom fields (EAV pattern via `CategoryField` / `PlaceFieldValue`)
- Photo gallery per place
- Reservation system with race-safe daily-capacity check (SQL `UPDLOCK/HOLDLOCK`)
- ASP.NET Core Identity with **Admin** and **Customer** roles
- City color tint palette system

## Useful commands

| Task | Command |
|---|---|
| Build | `dotnet build` |
| Run | `dotnet run --project Ween` |
| Add a migration | `dotnet ef migrations add <Name> --project Ween` |
| Apply migrations | `dotnet ef database update --project Ween` |
| Re-scaffold business entities | See `CLAUDE.md` for the full scaffold command |

## Notes

- The business tables (`Places`, `Cities`, `Categories`, etc.) are **database-first** and were scaffolded from an existing schema. Do not delete or recreate them via migrations.
- Only the Identity tables (`AspNet*`) are managed by EF Core migrations.
- Uploaded place photos are stored under `wwwroot/uploads/` which is excluded from version control.
