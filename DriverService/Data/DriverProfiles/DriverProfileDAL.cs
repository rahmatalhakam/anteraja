using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DriverService.Dtos.DriverProfiles;
using DriverService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace DriverService.Data.DriverProfiles
{
    public class DriverProfileDAL : IDriverProfile
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DriverProfileDAL(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task Delete(string id)
        {
            var result = await GetById(id);
            if (result == null) throw new Exception("Profile not found");
            try
            {
                _context.DriverProfiles.Remove(result);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception($"Error: {dbEx.Message}");
            }
        }

        public async Task<IEnumerable<DriverProfile>> GetAll()
        {
            var results = await _context.DriverProfiles.Select(dp => dp).ToListAsync();

            return results;
        }

        public async Task<DriverProfile> GetById(string id)
        {
            var result = await _context.DriverProfiles.Where(s => s.DriverProfileId == Convert.ToInt32(id)).SingleOrDefaultAsync<DriverProfile>();
            if (result != null)
                return result;
            else
                throw new Exception("Profile not Found");
        }

        public async Task<DriverProfile> GetProfileById()
        {
            var currentDriverId = _httpContextAccessor.HttpContext.User.FindFirstValue("DriverId");
            var driver = _context.Users.Where(u => u.Id == currentDriverId).SingleOrDefault();
            if (driver == null)
            {
                throw new Exception("Account not found");
            }
            if (driver.LockoutEnabled)
            {
                throw new Exception("Cannot show profile, your account is locked");
            }

            var result = await _context.DriverProfiles.Where(s => s.DriverId == currentDriverId).SingleOrDefaultAsync<DriverProfile>();
            if (result != null)
                return result;
            else
                throw new Exception("Profile not Found, please set yout profile");

        }

        public async Task<DriverProfile> Insert(DriverProfile obj)
        {
            var currentDriverId = _httpContextAccessor.HttpContext.User.FindFirstValue("DriverId");
            var driver = _context.Users.Where(u => u.Id == currentDriverId).SingleOrDefault();
            if (driver == null)
            {
                throw new Exception("Account not found");
            }


            var data = new DriverProfile
            {
                DriverId = currentDriverId,
                FirstName = obj.FirstName,
                LastName = obj.LastName,
            };

            try
            {

                _context.DriverProfiles.Add(data);
                await _context.SaveChangesAsync();
                return data;

            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception($"Error: {dbEx.Message}");
            }
        }

        public async Task<DriverProfile> SetPosition(DriverProfile input)
        {
            var currentDriverId = _httpContextAccessor.HttpContext.User.FindFirstValue("DriverId");
            var driver = _context.Users.Where(u => u.Id == currentDriverId).SingleOrDefault();
            if (driver == null)
            {
                throw new Exception("Account not found");
            }
            if (driver.LockoutEnabled)
            {
                throw new Exception("Cannot set location, your account is locked");
            }

            try
            {
                var result = await GetById(currentDriverId);
                if (result == null)
                {
                    throw new Exception($"Cannot set Position, please set your profile first.");
                }

                var data = new DriverProfile
                {
                    DriverProfileId = result.DriverProfileId,
                    DriverId = result.DriverId,
                    FirstName = result.FirstName,
                    LastName = result.LastName,
                    LongNow = input.LongNow,
                    LatNow = input.LatNow
                };

                _context.Entry(result).CurrentValues.SetValues(data);
                await _context.SaveChangesAsync();

                return data;
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception($"Error: {dbEx.Message}");
            }
        }

        public async Task<DriverProfile> Update(string id, DriverProfile obj)
        {
            try
            {
                var result = await GetById(id);
                if (result == null)
                {
                    throw new Exception($"Profile with Id {id} not found");
                }
                result.FirstName = obj.FirstName;
                result.LastName = obj.LastName;
                result.LongNow = obj.LongNow;
                result.LatNow = obj.LatNow;
                await _context.SaveChangesAsync();
                obj.DriverProfileId = Convert.ToInt32(id);
                obj.DriverId = result.DriverId;
                return obj;
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception($"Error: {dbEx.Message}");
            }
        }
    }
}