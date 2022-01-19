using System.Collections.Generic;
using System.Threading.Tasks;
using TransactionService.Models;

namespace TransactionService.Data
{
  public interface IPrice
  {
    Task<Price> Insert(Price obj);
    Task<Price> Update(int id, Price obj);
    Task<IEnumerable<Price>> GetAll();
    Task<Price> GetById(int id);

  }
}