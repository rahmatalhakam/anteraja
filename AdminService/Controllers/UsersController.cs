using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminService.Data;
using AdminService.Dtos;
using AdminService.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AdminService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private IUser _user;
        private IMapper _mapper;

        public UsersController(IUser user, IMapper mapper)
        {
             _user = user;
             _mapper = mapper;
            
        }

         [AllowAnonymous]
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

       

        [AllowAnonymous]
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