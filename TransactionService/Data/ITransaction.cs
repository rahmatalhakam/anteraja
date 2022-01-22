using System.Collections.Generic;
using System.Threading.Tasks;
using TransactionService.Models;

namespace TransactionService.Data
{
  public interface ITransaction
  {
    Task<Transaction> Insert(string area, int distance, Transaction obj);
    Task<Transaction> UpdateStatus(int transactionId, string driverId);
    Task<IEnumerable<Transaction>> GetAll();
    Task<Transaction> GetById(int id);
    Task<IEnumerable<Transaction>> GetByCustomerId(string id, string rolename);
  }
}