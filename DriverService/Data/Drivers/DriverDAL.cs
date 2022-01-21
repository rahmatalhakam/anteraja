using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DriverService.Dtos;
using DriverService.Helpers;
using DriverService.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using DriverService.Dtos.Drivers;

namespace DriverService.Data.Drivers
{
    public class DriverDAL : IDriver
    {
        private UserManager<IdentityUser> _userManager;

        private RoleManager<IdentityRole> _roleManager;
        private AppSettings _appSettings;

        public DriverDAL(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager,
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

        public async Task<Driver> Authenticate(LoginInput input)
        {
            var account = await _userManager.FindByNameAsync(input.Username);
            if (account == null)
            {
                return null;
            }
            var userFind = await _userManager.CheckPasswordAsync(
              account, input.Password);
            if (!userFind)
            {
                return null;
            }
            if (account.LockoutEnabled)
            {
                throw new Exception("Cannot Login, your account is Locked");
            }
            var user = new Driver
            {
                Id = account.Id,
                Username = input.Username
            };

            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, user.Username));
            claims.Add(new Claim("DriverId", user.Id.ToString()));

            var roles = await GetRolesFromUser(input.Username);
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
                users.Add(new UsernameOutput { Username = result.UserName });
            }
            return users;
        }

        public async Task<List<string>> GetRolesFromUser(string username)
        {
            List<string> roles = new List<string>();
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
                throw new Exception($"User {username} not found");
            var results = await _userManager.GetRolesAsync(user);
            foreach (var result in results)
            {
                roles.Add(result);
            }
            return roles;
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
                Username = user.UserName
            };

            return data;

        }

        public async Task LockUser(LockUserInput input)
        {
            var user = await _userManager.FindByNameAsync(input.Username);
            if (user == null)
            {
                throw new Exception($"User {input.Username} not found");
            }

            var lockUser = await _userManager.SetLockoutEnabledAsync(user, input.isLock);

        }

        public async Task Registration(RegisterInput user)
        {
            try
            {
                // Lockout = false; not locked;
                var newUser = new IdentityUser { UserName = user.Username, Email = user.Email };
                var result = await _userManager.CreateAsync(newUser, user.Password);
                var userFind = await _userManager.FindByNameAsync(user.Username);
                await _userManager.SetLockoutEnabledAsync(userFind, true);


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
