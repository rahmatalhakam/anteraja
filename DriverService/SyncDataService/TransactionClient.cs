using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using DriverService.Helpers;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Net;
using DriverService.Dtos.Orders;

namespace DriverService.SyncDataService
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

        public async Task<int> GetDistance(DistanceInput input, string token)
        {
            HttpClientHandler handler = new HttpClientHandler();
            using (var client = new HttpClient(handler, false))

            {
                var json = JsonSerializer.Serialize(input);

                client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);
                var response = await client.GetAsync(_appSettings.TransactionUrl + $"/api/v1/Transactions/distance?lat1={input.lat1}&long1={input.long1}&lat2={input.lat2}&long2={input.long2}");
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

                return Convert.ToInt32(content);
            }
        }

        public async Task<CreateTransactionOutput> PostTransaction(CreateTransactionInput input, string token)
        {
            HttpClientHandler handler = new HttpClientHandler();
            using (var client = new HttpClient(handler, false))
            {
                var json = JsonSerializer.Serialize(input);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);
                var response = await client.PostAsync(_appSettings.TransactionUrl + "/api/v1/Transactions", data);
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
                CreateTransactionOutput responseData = JsonSerializer.Deserialize<CreateTransactionOutput>(content);
                Console.WriteLine(JsonSerializer.Serialize<CreateTransactionOutput>(responseData));
                return responseData;
            }
        }
    }
}