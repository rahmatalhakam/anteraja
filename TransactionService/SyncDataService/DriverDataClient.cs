using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TransactionService.Dtos;

namespace TransactionService.SyncDataService
{
  public class DriverDataClient : IDriver
  {
    // TODO: buat get by id
    public Task<DriverOutput> GetById(int UserId)
    {
      return null;
    }
  }
}