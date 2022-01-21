using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DriverService.Models;
using Microsoft.AspNetCore.Identity;
using DriverService.Dtos.Drivers;

namespace DriverService.Data.Drivers
{
    public interface IDriver
    {
        IEnumerable<UsernameOutput> GetAllUser();
        Task Registration(RegisterInput user);
        Task AddRole(string rolename);
        IEnumerable<RoleOutput> GetAllRole();
        Task AddRoleForUser(string username, string role);
        Task<List<string>> GetRolesFromUser(string username);
        Task<Driver> Authenticate(LoginInput input);
        Task LockUser(LockUserInput input);
        Task<UsernameOutput> GetUserById(string id);
    }
}
