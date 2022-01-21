using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DriverService.Models;

namespace DriverService.Data.Orders
{
  public interface IOrder
  {

    Task<Order> Insert(Order user);
    Task<IEnumerable<Order>> GetOrders();

  }
}
