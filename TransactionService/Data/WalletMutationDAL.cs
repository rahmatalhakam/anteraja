using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TransactionService.Models;

namespace TransactionService.Data
{
  public class WalletMutationDAL : IWalletMutation
  {
    private readonly AppDbContext _db;

    public WalletMutationDAL(AppDbContext db)
    {
      _db = db;
    }
    public async Task<WalletMutation> GetById(int id)
    {
      var result = await _db.WalletMutations.FindAsync(id);
      return result;
    }

    // public async Task<WalletMutation> GetLastById(int id)
    // {
    //   var result = await _db.WalletMutations.FindAsync(id).Get;
    //   return result;
    // }

    public Task<WalletMutation> GetByWalletUserId(int id)
    {
      var result = _db.WalletMutations.Where(w => w.WalletUserId == id).OrderBy(w => w.Id).LastOrDefault();
      return Task.FromResult(result);
    }

    public async Task<WalletMutation> GetByCustomerId(string customerId)
    {
      var walletUser = await _db.WalletUsers.Where(w => w.CustomerId == customerId).Include(w => w.WalletMutations).FirstAsync();
      return walletUser.WalletMutations.OrderBy(m => m.Id).LastOrDefault();
    }


    public async Task<WalletMutation> Insert(WalletMutation obj)
    {
      var result = await _db.WalletMutations.AddAsync(obj);
      await _db.SaveChangesAsync();
      return result.Entity;
    }
  }
}