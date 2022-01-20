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
            return result;

        }

        public async Task<DriverProfile> Insert(DriverProfile obj)
        {
            var currentDriverId = _httpContextAccessor.HttpContext.User.FindFirstValue("DriverId");
            var driver = _context.Users.Where(u => u.Id == currentDriverId).SingleOrDefault();
            if (driver == null)
            {
                throw new Exception("Account not found");
            }

            var profile = GetProfileById().Result;
            try
            {
                var data = new DriverProfile
                {
                    DriverId = currentDriverId,
                    FirstName = obj.FirstName,
                    LastName = obj.LastName,

                };

                if (profile != null)
                {
                    profile.FirstName = obj.FirstName;
                    profile.LastName = obj.LastName;
                }
                else
                {
                    _context.DriverProfiles.Add(data);
                }

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
                throw new Exception("Cannot setup location, your account is locked");
            }


            try
            {
                var findProfile = GetProfileById().Result;
                if (findProfile == null)
                {
                    throw new Exception("Cannot set the location, setup your profile first");
                }

                findProfile.LongNow = input.LongNow;
                findProfile.LatNow = input.LatNow;
                await _context.SaveChangesAsync();
                input.DriverProfileId = findProfile.DriverProfileId;
                input.DriverId = findProfile.DriverId;

                return findProfile;

            }
            catch (Exception ex)
            {

                throw new Exception($"Error: {ex.Message}");
            }

        }

        public async Task<DriverProfile> Update(DriverProfile obj)
        {
            var currentDriverId = _httpContextAccessor.HttpContext.User.FindFirstValue("DriverId");
            var driver = _context.Users.Where(u => u.Id == currentDriverId).SingleOrDefault();
            if (driver == null)
            {
                throw new Exception("Account not found");
            }
            if (driver.LockoutEnabled)
            {
                throw new Exception("Cannot update profile, your account is locked");
            }

            try
            {
                var profile = GetProfileById().Result;
                if (profile == null)
                {
                    throw new Exception("Cannot update profile, setup your profile first");
                }
                profile.FirstName = obj.FirstName;
                profile.LastName = obj.LastName;
                profile.LongNow = obj.LongNow;
                profile.LatNow = obj.LatNow;
                await _context.SaveChangesAsync();
                obj.DriverProfileId = profile.DriverProfileId;
                obj.DriverId = profile.DriverId;
                return obj;
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception($"Error: {dbEx.Message}");
            }
        }
    }
}