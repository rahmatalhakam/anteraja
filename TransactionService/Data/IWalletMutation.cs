using System.Threading.Tasks;
using TransactionService.Models;

namespace TransactionService.Data
{
  public interface IWalletMutation
  {
    Task<WalletMutation> Insert(WalletMutation obj);
    Task<WalletMutation> GetById(int id);
    Task<WalletMutation> GetByWalletUserId(int id);
    Task<WalletMutation> GetByCustomerId(string customerId);
  }
}