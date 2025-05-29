using Microsoft.AspNetCore.Identity;

namespace MiniAccountSystem.Models
{
    public static class SeedData
    {
        public static async Task InitializeData(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roleNames = { "Admin", "Accountant", "Viewer" };

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                    await roleManager.CreateAsync(new IdentityRole(roleName));
            }

            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            string adminEmail = "admin@gmail.com";
            string adminPass = "Admin@123";

            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var user = new IdentityUser { UserName = adminEmail, Email = adminEmail };
                var result = await userManager.CreateAsync(user, adminPass);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }


            }
        }
    }
}
