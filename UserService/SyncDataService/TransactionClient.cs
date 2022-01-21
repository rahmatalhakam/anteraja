using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using UserService.Dtos.Transactions;
using UserService.Helpers;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Net;

namespace UserService.SyncDataService
{
  public class TransactionClient : ITransactionClient
  {

    private readonly HttpClient _httpClient;
    private readonly AppSettings _appSettings;

    public TransactionClient(HttpClient httpClient, IOptions<AppSettings> appSettings)
    {
      _httpClient = httpClient;
      _appSettings = appSettings.Value;
    }

    public async Task<FeeOutput> CheckOrderFee(FeeInput input, string token)
    {
      using (var client = _httpClient)
      {
        var json = JsonSerializer.Serialize(input);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        var response = await client.PostAsync(_appSettings.TransactionUrl + "/api/v1/Transactions/fee", data);
        var content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
          try
          {
            ErrorOutput error = JsonSerializer.Deserialize<ErrorOutput>(content);
            throw new Exception(error.Message);
          }
          catch (System.Exception)
          {
            if (response.StatusCode == HttpStatusCode.BadRequest)
              throw new Exception(await response.Content.ReadAsStringAsync());
            throw new Exception(response.StatusCode.ToString());
          }

        }
        Console.WriteLine("==>>>>> " + content);
        FeeOutput responseData = JsonSerializer.Deserialize<FeeOutput>(content);
        Console.WriteLine(JsonSerializer.Serialize<FeeOutput>(responseData));
        return responseData;
      }
    }

  }
}