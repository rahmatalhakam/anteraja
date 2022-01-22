using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DriverService.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace DriverService.Data
{
    public class DbInitializer
    {
        private AppDbContext _context;

        public DbInitializer(AppDbContext context)
        {
            _context = context;
        }

        public async Task Initialize(UserManager<IdentityUser> userManager)
        {
            var roleStore = new RoleStore<IdentityRole>(_context);

            if (userManager.FindByEmailAsync("admin@admin.com").Result == null)
            {
                if (!_context.Roles.Any(r => r.Name == "Admin"))
                {
                    await roleStore.CreateAsync(new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" });
                }
                if (!_context.Roles.Any(r => r.Name == "Driver"))
                {
                    await roleStore.CreateAsync(new IdentityRole { Name = "Driver", NormalizedName = "DRIVER" });
                }

                IdentityUser user = new IdentityUser
                {
                    UserName = "Admin",
                    Email = "admin@admin.com"
                };
                IdentityResult result = userManager.CreateAsync(user, "Admin@123").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                    userManager.SetLockoutEnabledAsync(user, false).Wait();
                }
                await _context.SaveChangesAsync();
            }
        }
    }
}