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
        Task<IEnumerable<Order>> GetOrders();
        Task<Order> GetOrderById(int id);
        Task<Order> AcceptOrder(OrderInput input);

    }
}
