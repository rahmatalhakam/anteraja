using Microsoft.EntityFrameworkCore;
using TransactionService.Models;

namespace TransactionService.Data
{
  public class AppDbContext : DbContext
  {
    public AppDbContext(DbContextOptions options) : base(options)
    {

    }

    public DbSet<Price> Prices { get; set; }

    public DbSet<StatusOrder> StatusOrders { get; set; }

    public DbSet<Transaction> Transactions { get; set; }

    public DbSet<WalletMutation> WalletMutations { get; set; }

    public DbSet<WalletUser> WalletUsers { get; set; }
  }
}