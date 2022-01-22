using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DriverService.Dtos.Orders;


namespace DriverService.SyncDataService
{
    public interface ITransactionClient
    {
        Task<int> GetDistance(DistanceInput input, string token);
        Task<CreateTransactionOutput> PostTransaction(CreateTransactionInput input, string token);
    }
}