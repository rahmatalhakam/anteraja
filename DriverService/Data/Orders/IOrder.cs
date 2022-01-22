using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DriverService.Dtos.Orders;
using DriverService.Models;

namespace DriverService.Data.Orders
{
    public interface IOrder
    {

        Task<Order> Insert(Order user);
        Task Delete(int id);
        Task<IEnumerable<Order>> GetOrders();
        Task<Order> GetById(int id);
        Task<Order> CheckOrder(AcceptOrderInput input);

    }
}
