using DriverService.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DriverService.Data
{
  public class AppDbContext : IdentityDbContext
  {
    public AppDbContext(DbContextOptions options) : base(options)
    {

    }

    public DbSet<DriverProfile> DriverProfiles { get; set; }
    public DbSet<Order> Orders { get; set; }

  }
}
