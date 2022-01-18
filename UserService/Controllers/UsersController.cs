using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Dtos.Users;
using UserService.Data.Users;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private IUser _user;

        public UsersController(IUser user)
        {
            _user = user;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Registration([FromBody] RegisterInput user)
        {
            try
            {
                await _user.Registration(user);
                await _user.AddRoleForUser(user.Username, "User");

                return Ok($"Register for user {user.Username} success");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult<UsernameOutput> GetAll()
        {
            try
            {
                var results = _user.GetAllUser();
                return Ok(results);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("Role")]
        public async Task<ActionResult> AddRole([FromBody] RoleOutput rolename)
        {
            try
            {
                await _user.AddRole(rolename.Rolename);
                return Ok($"Role {rolename.Rolename} not found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("Role")]
        public ActionResult<IEnumerable<RoleOutput>> GetAllRole()
        {
            try
            {
                return Ok(_user.GetAllRole());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("UserInRole")]
        public async Task<ActionResult> AddRoleForUser(string username, string role)
        {
            try
            {
                await _user.AddRoleForUser(username, role);
                return Ok($"Data {username} and {role} successfully added");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{username}/Role")]
        public async Task<ActionResult<List<string>>> GetRolesFromUser(string username)
        {
            try
            {
                var results = await _user.GetRolesFromUser(username);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("Authentication")]
        public async Task<ActionResult<User>> Authentication(LoginInput input)
        {
            try
            {
                var user = await _user.Authenticate(input.Username, input.Password);
                if (user == null) return BadRequest("username/password incorrect");
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }
        [AllowAnonymous]
        [HttpPost("Lock")]
        public async Task<ActionResult> LockUser(string username, bool isLock)
        {
            try
            {
                await _user.LockUser(username, isLock);
                return Ok($"{username} Lock privilage is successfully updated to {isLock}");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetUserById(string id)
        {
            {
                try
                {
                    var result = await _user.GetUserById(id);
                    return Ok(result);
                }
                catch (Exception ex)
                {

                    return BadRequest(ex.Message);
                }
            }
        }
    }
}