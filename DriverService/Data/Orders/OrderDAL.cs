using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DriverService.Dtos.DriverProfiles;
using DriverService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace DriverService.Data.Orders
{
  public class OrderDAL : IOrder
  {
    private readonly AppDbContext _context;

    public OrderDAL(AppDbContext context)
    {
      _context = context;
    }

    public async Task<IEnumerable<Order>> GetOrders()
    {
      var result = await _context.Orders.ToListAsync();
      return result;
    }

    public async Task<Order> Insert(Order user)
    {
      var result = await _context.Orders.AddAsync(user);
      await _context.SaveChangesAsync();
      return result.Entity;
    }
  }
}