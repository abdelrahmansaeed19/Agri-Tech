using AgriculturalTech.API.Data;
using AgriculturalTech.API.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace AgriculturalTech.API.Helpers
{
    public static class SeedDataHelper
    {
        public static async Task InitializeAsync(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            // Ensure database is created
            await context.Database.EnsureCreatedAsync();

            // Seed Roles
            string[] roleNames = { "Admin", "Farmer", "Agronomist", "Viewer" };

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Seed Admin User
            var adminEmail = "admin@agritech.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "System Administrator",
                    FarmName = "Admin Farm",
                    FarmLocation = "Cairo, Egypt",
                    EmailConfirmed = true,
                    IsActive = true,
                    PreferredLanguage = "en"
                };

                var result = await userManager.CreateAsync(adminUser, "Admin@123456");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            // Seed Test Farmer
            var farmerEmail = "farmer@test.com";
            var farmerUser = await userManager.FindByEmailAsync(farmerEmail);

            if (farmerUser == null)
            {
                farmerUser = new ApplicationUser
                {
                    UserName = farmerEmail,
                    Email = farmerEmail,
                    FullName = "Test Farmer",
                    FarmName = "Green Valley Farm",
                    FarmLocation = "Cairo, Egypt",
                    FarmAreaInAcres = 50,
                    EmailConfirmed = true,
                    IsActive = true,
                    PreferredLanguage = "en"
                };

                var result = await userManager.CreateAsync(farmerUser, "Farmer@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(farmerUser, "Farmer");
                }
            }

            await context.SaveChangesAsync();
        }
    }
}
