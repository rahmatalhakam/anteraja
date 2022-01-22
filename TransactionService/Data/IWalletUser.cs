using System.Threading.Tasks;
using TransactionService.Models;

namespace TransactionService.Data
{
  public interface IWalletUser
  {
    Task<WalletUser> Insert(WalletUser obj);
    Task<WalletUser> GetById(int id);
    Task<WalletUser> GetByCustomerId(string id);
  }
}