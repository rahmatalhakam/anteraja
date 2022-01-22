using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TransactionService.Models;

namespace TransactionService.Data
{
  public class TransactionDAL : ITransaction
  {
    private AppDbContext _db;
    public TransactionDAL(AppDbContext db)
    {
      _db = db;
    }
    public async Task<IEnumerable<Transaction>> GetAll()
    {
      var result = await _db.Transactions.Include(t => t.StatusOrder).Include(t => t.Price).ToListAsync();
      return result;
    }

    public async Task<IEnumerable<Transaction>> GetByCustomerId(string id, string rolename)
    {
      if (rolename == "Driver")
      {
        var result = await _db.Transactions.Include(t => t.StatusOrder).Where(t => t.DriverId == id).ToListAsync();
        return result;
      }
      else if (rolename == "User")
      {
        var result = await _db.Transactions.Include(t => t.StatusOrder).Where(t => t.UserId == id).ToListAsync();
        return result;
      }
      throw new System.Exception($"Customer id: {id} with rolename: {rolename} not found");
    }

    public async Task<Transaction> GetById(int id)
    {
      var result = await _db.Transactions.Include(t => t.StatusOrder).Where(t => t.Id == id).SingleAsync();
      return result;
    }

    public async Task<Transaction> Insert(string area, int distance, Transaction obj)
    {
      try
      {
        var price = _db.Prices.Where(p => p.Area.ToLower().Contains(area.ToLower())).SingleOrDefault();
        if (price == null)
        {
          obj.PriceId = 1; //base price
          obj.Billing = distance * _db.Prices.FindAsync(1).Result.PricePerKM;
        }
        else
        {
          obj.PriceId = price.Id;
          obj.Billing = distance * price.PricePerKM;
        }
        obj.StatusOrder = _db.StatusOrders.FindAsync(1).Result;
        obj.CreatedAt = System.DateTime.Now;
        var result = await _db.Transactions.AddAsync(obj);
        await _db.SaveChangesAsync();
        return result.Entity;
      }
      catch (System.Exception)
      {
        throw;
      }
    }

    public async Task<Transaction> UpdateStatus(int transactionId, string driverId)
    {
      var transaction = await _db.Transactions.Where(t => t.Id == transactionId && t.DriverId == driverId).FirstAsync();
      if (transaction == null)
        throw new System.Exception($"Transaction with id: {transactionId} and driver id: {driverId} not found");
      transaction.StatusOrderId = 2;
      transaction.StatusOrder = _db.StatusOrders.FindAsync(2).Result;
      _db.Transactions.Update(transaction);
      await _db.SaveChangesAsync();
      return transaction;
    }


  }
}