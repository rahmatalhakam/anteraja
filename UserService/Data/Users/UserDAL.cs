using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UserService.Dtos;
using UserService.Helpers;
using UserService.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using UserService.Dtos.Users;

namespace UserService.Data.Users
{
    public class UserDAL : IUser
    {
        private UserManager<IdentityUser> _userManager;

        private RoleManager<IdentityRole> _roleManager;
        private AppSettings _appSettings;

        public UserDAL(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager,
            IOptions<AppSettings> appSettings)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _appSettings = appSettings.Value;
        }

        public async Task AddRole(string rolename)
        {
            IdentityResult roleResult;
            try
            {
                var roleIsExist = await _roleManager.RoleExistsAsync(rolename);
                if (roleIsExist)
                    throw new Exception($"Role {rolename} already Exists");
                roleResult = await _roleManager.CreateAsync(new IdentityRole(rolename));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task AddRoleForUser(string username, string role)
        {
            var user = await _userManager.FindByNameAsync(username);
            try
            {
                var result = await _userManager.AddToRoleAsync(user, role);
                if (!result.Succeeded)
                {

                    StringBuilder errMsg = new StringBuilder(String.Empty);
                    foreach (var err in result.Errors)
                    {
                        errMsg.Append(err.Description + " ");
                    }
                    throw new Exception($"{errMsg}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<User> Authenticate(string username, string password)
        {
            var account = await _userManager.FindByNameAsync(username);
            if (account == null)
            {
                return null;
            }
            var userFind = await _userManager.CheckPasswordAsync(
              account, password);
            if (!userFind)
            {
                return null;
            }
            if (account.LockoutEnabled)
            {
                throw new Exception("Cannot Login, your account is Locked");
            }
            var user = new User
            {
                Id = account.Id,
                Username = username

            };

            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, user.Username));

            var roles = await GetRolesFromUser(username);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)

            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);
            return user;
        }

        public IEnumerable<RoleOutput> GetAllRole()
        {
            List<RoleOutput> roles = new List<RoleOutput>();
            var results = _roleManager.Roles;
            foreach (var result in results)
            {
                roles.Add(new RoleOutput { Rolename = result.Name });
            }
            return roles;
        }


        public IEnumerable<UsernameOutput> GetAllUser()
        {
            List<UsernameOutput> users = new List<UsernameOutput>();
            var results = _userManager.Users;
            foreach (var result in results)
            {
                var roles = GetRolesFromUser(result.UserName).Result;
                users.Add(new UsernameOutput { Username = result.UserName, Id = result.Id, Role = roles });
            }
            return users;
        }

        public async Task<List<string>> GetRolesFromUser(string username)
        {
            List<string> listRoles = new List<string>();

            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                throw new Exception($"{username} does not exist");
            }
            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                listRoles.Add(role);
            }
            return listRoles;
        }

        public async Task<UsernameOutput> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new Exception($"User with Id {id} not found");
            }
            if (user.LockoutEnabled)
            {
                throw new Exception($"User is locked");
            }

            var data = new UsernameOutput
            {
                Id = user.Id,
                Username = user.UserName,
                Role = GetRolesFromUser(user.UserName).Result
            };

            return data;

        }

        public async Task LockUser(string username, bool isLock)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                throw new Exception($"User {username} not found");
            }

            var lockUser = await _userManager.SetLockoutEnabledAsync(user, isLock);

        }

        public async Task Registration(RegisterInput user)
        {
            try
            {
                // Lockout = false; not locked;
                var newUser = new IdentityUser { UserName = user.Username, Email = user.Email, LockoutEnabled = false };
                var result = await _userManager.CreateAsync(newUser, user.Password);
                var userFind = await _userManager.FindByNameAsync(user.Username);
                await _userManager.SetLockoutEnabledAsync(userFind, false);


                if (!result.Succeeded)
                {
                    StringBuilder errMsg = new StringBuilder(String.Empty);
                    foreach (var err in result.Errors)
                    {
                        errMsg.Append(err.Description + " ");
                    }
                    throw new Exception($"{errMsg}");
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }


    }
}
