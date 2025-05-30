using Microsoft.AspNetCore.Identity;
using MiniAccountSystem.Services;

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

            var accessService = serviceProvider.GetRequiredService<ModuleAccessService>();
            await accessService.GrantAccessAsync("Admin", "Index");
            await accessService.GrantAccessAsync("Admin", "Create");
            await accessService.GrantAccessAsync("Admin", "Edit");
            await accessService.GrantAccessAsync("Admin", "Delete");
            await accessService.GrantAccessAsync("Admin", "Details");


           
            // Create accountant
            string accountantEmail = "accountant@gmail.com";
            string accountantPass = "Accountant@123";
            if (await userManager.FindByEmailAsync(accountantEmail) == null)
            {
                var user = new IdentityUser { UserName = accountantEmail, Email = accountantEmail };
                if ((await userManager.CreateAsync(user, accountantPass)).Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Accountant");

                  
                    await accessService.GrantAccessAsync("Accountant", "Index");
                    await accessService.GrantAccessAsync("Accountant", "Create");
                    await accessService.GrantAccessAsync("Accountant", "Details");
                    await accessService.GrantAccessAsync("Accountant", "CreateVoucher");
                }
            }
        



            if (!await roleManager.RoleExistsAsync("Viewer"))
            {
                await roleManager.CreateAsync(new IdentityRole("Viewer"));
                
            }
            await accessService.GrantAccessAsync("Viewer", "Index");
            await accessService.GrantAccessAsync("Viewer", "Details");


        }
    }
}
