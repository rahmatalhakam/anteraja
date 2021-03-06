using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using TransactionService.Dtos;
using TransactionService.Helpers;

namespace TransactionService.SyncDataService
{
  public class DriverDataClient : IDriverDataClient
  {
    // TODO: buat get by id

    private readonly HttpClient _httpClient;
    private readonly AppSettings _appSettings;

    public DriverDataClient(HttpClient httpClient, IOptions<AppSettings> appSettings)
    {
      _httpClient = httpClient;
      _appSettings = appSettings.Value;
    }
    public async Task<bool> GetById(string UserId)
    {
      var response = await _httpClient.GetAsync(_appSettings.DriverUrl + "/api/Drivers/" + UserId);
      Console.WriteLine(response.StatusCode);
      if (response.IsSuccessStatusCode)
      {
        return true;
      }
      else
      {
        return false;
      }
    }
  }
}