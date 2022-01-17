using System;
using System.Linq;
using System.Threading.Tasks;
using TransactionService.Models;

namespace TransactionService.Data
{
  public static class DbInitilizer
  {
    public static void Initilize(AppDbContext context)
    {
      context.Database.EnsureCreated();
      if (context.Prices.Any())
      {
        return;
      }
      else
      {
        context.Prices.Add(new Price { PricePerKM = 3000, Area = "BASE" });
        context.SaveChanges();
      }

      if (context.StatusOrders.Any())
      {
        return;
      }
      else
      {
        var statusOrders = new StatusOrder[]
      {
        new StatusOrder{Status="On Progress"},
        new StatusOrder{Status="Finished"},

      };
        foreach (var statusOrder in statusOrders)
        {
          context.StatusOrders.Add(statusOrder);
        }
        context.SaveChanges();
      }

    }

  }
}
