using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminService.Data;
using AdminService.Dtos;
using AdminService.Models;
using Microsoft.AspNetCore.Mvc;

namespace AdminService.Controllers
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

         [HttpPost]
        public async Task<ActionResult> Registration([FromBody] RegisterInput user)
        {
            try
            {
                await _user.Registration(user);
                await _user.AddRoleForUser(user.Username, "Admin");
                
                return Ok($"Registrasi user {user.Username} berhasil");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

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

        // [HttpPost("Role")]
        // public async Task<ActionResult> AddRole([FromBody] RoleOutput rolename)
        // {
        //     try
        //     {
        //         await _user.AddRole(rolename.Rolename);
        //         return Ok($"Tambah role {UserRole.rolename} berhasil");
        //     }
        //     catch (Exception ex)
        //     {
        //         return BadRequest(ex.Message);
        //     }
        // }

        // [HttpPost("UserInRole")]
        // public async Task<ActionResult> AddRoleForUser(string username, string role)
        // {
        //     try
        //     {
        //         await _user.AddRoleForUser(username, role);
        //         return Ok($"Data {username} and {role} successfully added");
        //     }
        //     catch (Exception ex)
        //     {
        //         return BadRequest(ex.Message);
        //     }
        // }

        [HttpPost("Authentication")]
        public async Task<ActionResult<User>> Authentication(LoginInput input)
        {
            try
            {
                var user = await _user.Authenticate(input.Username, input.Password);
                if (user == null) return BadRequest("username/password tidak tepat");
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }

    }
}