using Microsoft.AspNetCore.Identity;

namespace Ween.Data;

public static class IdentitySeeder
{
    public const string AdminEmail = "admin@ween.local";
    public const string AdminPassword = "Admin#123";

    public static readonly string[] Roles = { "Admin", "Customer" };

    public static async Task SeedAsync(IServiceProvider services)
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole<int>>>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

        foreach (var role in Roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole<int>(role));
        }

        if (await userManager.FindByEmailAsync(AdminEmail) is null)
        {
            var admin = new ApplicationUser
            {
                UserName = AdminEmail,
                Email = AdminEmail,
                EmailConfirmed = true,
                FullName = "Site Admin",
                PhoneNumber = "+962000000000",
                CreatedAt = DateTime.UtcNow
            };

            var result = await userManager.CreateAsync(admin, AdminPassword);
            if (result.Succeeded)
                await userManager.AddToRoleAsync(admin, "Admin");
        }
    }
}
