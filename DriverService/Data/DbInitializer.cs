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

        public async Task Initialize()
        {
            var roleStore = new RoleStore<IdentityRole>(_context);

            if (!_context.Roles.Any(r => r.Name == "Admin"))
            {
                await roleStore.CreateAsync(new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" });
            }
            if (!_context.Roles.Any(r => r.Name == "Driver"))
            {
                await roleStore.CreateAsync(new IdentityRole { Name = "Driver", NormalizedName = "DRIVER" });
            }
            await _context.SaveChangesAsync();

        }
    }
}