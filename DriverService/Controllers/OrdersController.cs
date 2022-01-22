using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using DriverService.Data.Orders;
using DriverService.Dtos.Orders;
using DriverService.Models;
using DriverService.SyncDataService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DriverService.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class OrdersController : ControllerBase
  {
    private readonly IOrder _order;
    private readonly ITransactionClient _transactionClient;

    public OrdersController(IOrder order, ITransactionClient transactionClient)
    {
      _order = order;
      _transactionClient = transactionClient;

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
    public async Task<ActionResult> AcceptOrder([FromBody] AcceptOrderInput input)
    {

      var result = await _order.CheckOrder(input);
      Console.WriteLine("latlong==>" + result.LatStart + " " + result.LatEnd);
      var GetData = new DistanceInput
      {
        lat1 = input.LatNow,
        long1 = input.LongNow,
        lat2 = result.LatStart,
        long2 = result.LongStart
      };

      //Check Distance
      string token = Request.Headers["Authorization"];
      string[] tokenWords = token.Split(' ');
      var distanceResult = await _transactionClient.GetDistance(GetData, tokenWords[1]);

      if (distanceResult > 5)
      {
        return Ok(new { Message = "Cannot Accept the order because your distance from the Customer is over 5 Kilometers", distance = distanceResult });
      }

      // Accepted
      // Post to make new Transaction
      var PostData = new CreateTransactionInput
      {
        UserId = result.UserId,
        DriverId = input.DriverId,
        LatStart = result.LatStart,
        LongStart = result.LongStart,
        LatEnd = result.LatEnd,
        LongEnd = result.LongEnd,
        Area = result.Area
      };

      var postOrderResult = await _transactionClient.PostTransaction(PostData, tokenWords[1]);
      await _order.Delete(result.OrderId);

      return Ok(new SuccessOutput { Message = "Accepted Order Successfully", Data = postOrderResult });

    }

  }

}