using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using spr311_web_api.DAL.Entities.Identity;
using spr311_web_api.DAL.settings;

namespace spr311_web_api.DAL.Intializer
{
    public static class Seeder
    {
        public static async void Seed(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                await context.Database.MigrateAsync();

                // roles
                if (!await roleManager.RoleExistsAsync(RoleSettings.AdminRole))
                {
                    var adminRole = new AppRole
                    {
                        Name = RoleSettings.AdminRole
                    };

                    await roleManager.CreateAsync(adminRole);
                }

                if (!await roleManager.RoleExistsAsync(RoleSettings.UserRole))
                {
                    var userRole = new AppRole
                    {
                        Name = RoleSettings.UserRole
                    };

                    await roleManager.CreateAsync(userRole);
                }

                // users
                if (await userManager.FindByNameAsync("admin") == null)
                {
                    var admin = new AppUser
                    {
                        Email = "admin@gmail.com",
                        UserName = "admin",
                        LastName = "admin",
                        FirstName = "admin",
                        EmailConfirmed = true
                    };

                    await userManager.CreateAsync(admin, "qwerty");

                    await userManager.AddToRoleAsync(admin, RoleSettings.AdminRole);
                }

                if (await userManager.FindByNameAsync("user") == null)
                {
                    var user = new AppUser
                    {
                        Email = "user@gmail.com",
                        UserName = "user",
                        LastName = "user",
                        FirstName = "user",
                        EmailConfirmed = true
                    };

                    await userManager.CreateAsync(user, "qwerty");

                    await userManager.AddToRoleAsync(user, RoleSettings.UserRole);
                }
            }
        }
    }
}
