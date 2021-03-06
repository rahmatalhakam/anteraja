using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DriverService.Dtos.DriverProfiles;
using DriverService.Dtos.Orders;
using DriverService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DriverService.Data.Orders
{
    public class OrderDAL : IOrder
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderDAL(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;

        }

        public Task<Order> CheckOrder(AcceptOrderInput input)
        {
            var currentDriverId = _httpContextAccessor.HttpContext.User.FindFirstValue("DriverId");
            var driver = _context.Users.Where(u => u.Id == currentDriverId).SingleOrDefault();
            if (driver == null)
            {
                throw new Exception("Account not found");
            }
            if (driver.LockoutEnabled)
            {
                throw new Exception("Cannot accept order, your account is locked. Please Contact Admin");
            }

            var profile = _context.DriverProfiles.Where(dp => dp.DriverId == driver.Id).SingleOrDefault();
            if (profile == null)
            {
                throw new Exception("Cannot accept order, please set your Profile first");
            }
            if (profile.LatNow == 0 && profile.LongNow == 0)
            {
                throw new Exception("Cannot accept order, please set your current position on your Profile first");
            }
            else
            {
                profile.LatNow = input.LatNow;
                profile.LongNow = input.LongNow;
                _ = _context.SaveChangesAsync();
            }

            var orderFind = GetById(input.OrderId);

            return orderFind;
        }

        public async Task Delete(int id)
        {
            var result = await GetById(id);
            if (result == null) throw new Exception("Orders not found");
            try
            {
                _context.Orders.Remove(result);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception($"Error: {dbEx.Message}");
            }
        }

        public async Task<Order> GetById(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                throw new Exception("Order not Found");
            }
            return order;
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