using System.Collections.Generic;
using System.Threading.Tasks;
using TransactionService.Dtos;
using TransactionService.Models;

namespace TransactionService.SyncDataService
{
  public interface IDriver
  {
    Task<DriverOutput> GetById(int UserId);
  }
}