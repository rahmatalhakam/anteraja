using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TransactionService.Models;

namespace TransactionService.Data
{
  public class PriceDAL : IPrice
  {
    private readonly AppDbContext _db;

    public PriceDAL(AppDbContext db)
    {
      _db = db;
    }
    public async Task<IEnumerable<Price>> GetAll()
    {
      var result = await _db.Prices.ToListAsync();
      return result;
    }

    public async Task<Price> GetById(int id)
    {
      var result = await _db.Prices.FindAsync(id);
      if (result == null) throw new System.Exception($"Price with id: {id} not found");
      return result;
    }

    public async Task<Price> Insert(Price obj)
    {
      var result = await _db.Prices.AddAsync(obj);
      await _db.SaveChangesAsync();
      return result.Entity;
    }

    public async Task<Price> Update(int id, Price obj)
    {
      var result = await GetById(id);
      if (result == null) throw new System.Exception($"Price with id: {id} not found");
      result.PricePerKM = obj.PricePerKM;
      result.Area = obj.Area;
      _db.Prices.Update(result);
      await _db.SaveChangesAsync();
      return result;
    }
  }
}