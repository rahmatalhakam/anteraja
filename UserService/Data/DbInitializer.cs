using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace UserService.Data
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
            if (!_context.Roles.Any(r => r.Name == "User"))
            {
                await roleStore.CreateAsync(new IdentityRole { Name = "User", NormalizedName = "USER" });
            }
            await _context.SaveChangesAsync();

        }
    }
}