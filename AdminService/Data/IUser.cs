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
        IEnumerable<UsernameOutput> GetAllUser();
        Task Registration(RegisterInput user);
        Task<UsernameOutput> GetUserById(string id);   
        Task AddRole(string rolename);
        IEnumerable<RoleOutput> GetAllRole();
        Task AddRoleForUser(string username, string role);
        Task<List<string>> GetRolesFromUser(string username);
        Task<User> Authenticate(string username, string password);
        Task Update(string id, RegisterInput user);
    }
}