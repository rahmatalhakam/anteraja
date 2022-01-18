using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminService.Dtos;
using AdminService.Models;
using Microsoft.EntityFrameworkCore;

namespace AdminService.Data
{
    public class AdminDAL : IAdmin
    {
        private AppDbContext _db;
        public AdminDAL(AppDbContext db)
        {
            _db = db;
        }

       public async Task Delete(string id)
        {
            try
            {
                var result = await GetAdminById(id);
                if (result == null) throw new Exception($"data admin {id} tidak ditemukan");
                _db.Admins.Remove(result);
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception($"error: {dbEx.Message}");
            }

        }

       public Task<IEnumerable<Admin>> GetAll()
        {
           throw new NotImplementedException();
        }

        public async Task<Admin> GetAdminById(string id)
        {
            var result = await _db.Admins.Where(s => s.AdminId == Convert.ToInt32(id)).SingleOrDefaultAsync();
            if (result != null)
                return result;
            else
                throw new Exception("Data tidak ditemukan !");
        }

        public Task<Admin> Insert(Admin obj)
        {
            throw new NotImplementedException();
        }

        public async Task<Admin> Update(string id, Admin obj)
        {
            try
            {
                var result = await GetAdminById(id);
                if (result == null) throw new Exception($"data admin id {id} tidak ditemukan");
                result.FirstName = obj.FirstName;
                result.LastName = obj.LastName;
                result.Email = obj.Email;
                result.Password = obj.Password;
                await _db.SaveChangesAsync();
                return result;
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception(dbEx.Message);
            }
        }
    }
}