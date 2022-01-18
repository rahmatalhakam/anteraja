using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DriverService.Models;
using Microsoft.AspNetCore.Identity;
using DriverService.Dtos.Users;

namespace DriverService.Data.Users
{
    public interface IUser
    {
        IEnumerable<UsernameOutput> GetAllUser();
        Task Registration(RegisterInput user);
        Task AddRole(string rolename);
        IEnumerable<RoleOutput> GetAllRole();
        Task AddRoleForUser(string username, string role);
        Task<List<string>> GetRolesFromUser(string username);
        Task<User> Authenticate(LoginInput input);
        Task LockUser(LockUserInput input);
        Task<UsernameOutput> GetUserById(string id);
    }
}
