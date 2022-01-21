using System.Collections.Generic;
using System.Threading.Tasks;
using DriverService.Data.Orders;
using DriverService.Dtos.Orders;
using DriverService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DriverService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrder _order;

        public OrdersController(IOrder order)
        {
            _order = order;
        }

        [Authorize(Roles = "Driver")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetAll()
        {
            var result = await _order.GetOrders();
            return Ok(result);
        }

        [Authorize(Roles = "Driver")]
        [HttpPost]
        public async Task<ActionResult> AcceptOrder(OrderInput input)
        {
            var result = await _order.AcceptOrder(input);
            return Ok(new { Message = $"Accept Order for Order Id {result.UserId} Success", data = result });
        }

    }

}