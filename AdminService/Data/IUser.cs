using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminService.Dtos;
using AdminService.Models;

namespace AdminService.Data
{
    public interface IUser
    {
        IEnumerable<UsernameOutput> GetAllAdmins();
        Task Registration(RegisterInput user);
        Task AddRole(string rolename);
        //IEnumerable<RoleOutput> GetAllRole();
        //Task AddRoleForUser(UserRole input);
        Task<List<string>> GetRolesFromUser(string username);
        Task<User> Authenticate(string username, string password);
    }
}