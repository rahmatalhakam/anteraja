using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserService.Dtos;
using UserService.Models;
using Microsoft.AspNetCore.Identity;
using UserService.Dtos.Users;

namespace UserService.Data.Users
{
  public interface IUser
  {
    IEnumerable<UserOutput> GetAllUser();
    Task Registration(RegisterInput user);
    Task AddRole(string rolename);
    IEnumerable<RoleOutput> GetAllRole();
    Task AddRoleForUser(string username, string role);
    Task<List<string>> GetRolesFromUser(string username);
    Task<User> Authenticate(string username, string password);
    Task LockUser(string username, bool isLock);
    Task<UsernameOutput> GetUserById(string id);
  }
}
