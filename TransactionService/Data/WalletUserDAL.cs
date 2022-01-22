using System.Linq;
using System.Threading.Tasks;
using TransactionService.Models;

namespace TransactionService.Data
{
  public class WalletUserDAL : IWalletUser
  {
    private readonly AppDbContext _db;

    public WalletUserDAL(AppDbContext db)
    {
      _db = db;
    }

    public Task<WalletUser> GetByCustomerId(string customerId)
    {
      var result = _db.WalletUsers.Where(w => w.CustomerId == customerId).FirstOrDefault();
      return Task.FromResult(result);
    }

    public async Task<WalletUser> GetById(int id)
    {
      var result = await _db.WalletUsers.FindAsync(id);
      return result;
    }

    public async Task<WalletUser> Insert(WalletUser obj)
    {
      var walletUserObj = GetByCustomerId(obj.CustomerId);
      if (walletUserObj.Result != null)
        throw new System.Exception($"Customer id: {obj.CustomerId} is already registered.");
      var result = await _db.WalletUsers.AddAsync(obj);
      await _db.SaveChangesAsync();
      return result.Entity;
    }
  }
}