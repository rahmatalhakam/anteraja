using System.Linq;
using System.Threading.Tasks;
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
      var result = _db.WalletMutations.Where(w => w.WalletUserId == id).Last();
      return Task.FromResult(result);
    }

    public async Task<WalletMutation> Insert(WalletMutation obj)
    {
      var result = await _db.WalletMutations.AddAsync(obj);
      await _db.SaveChangesAsync();
      return result.Entity;
    }
  }
}