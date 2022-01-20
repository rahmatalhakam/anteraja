using System.Collections.Generic;
using System.Threading.Tasks;
using UserService.Dtos.Transactions;

namespace UserService.SyncDataService
{
  public interface ITransactionClient
  {
    Task<FeeOutput> CheckOrderFee(FeeInput input, string token);
  }
}