using DriverService.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DriverService.Data
{


  public class KafkaDbContext : DbContext
  {
    private readonly string _connString;
    public KafkaDbContext(string connString)
    {
      _connString = connString;
    }

    public DbSet<Order> Orders { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseSqlServer(_connString);
    }
  }
}
