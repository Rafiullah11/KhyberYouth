using KhyberYouth.Identity;
using KhyberYouth.Models;
using Microsoft.AspNetCore.Identity;
namespace KhyberYouth.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            context.Database.EnsureCreated();

            // Ensure roles are created
            string adminRole = "Admin";
            if (!await roleManager.RoleExistsAsync(adminRole))
            {
                await roleManager.CreateAsync(new IdentityRole(adminRole));
            }

            // Check if the admin user already exists
            string adminEmail = "admin@example.com";
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                // Set password and create user
                string adminPassword = "Admin@123"; // Secure password for production
                var createAdminResult = await userManager.CreateAsync(adminUser, adminPassword);

                if (createAdminResult.Succeeded)
                {
                    // Assign Admin role to the user
                    await userManager.AddToRoleAsync(adminUser, adminRole);
                }
            }

           
        }

    }
}
