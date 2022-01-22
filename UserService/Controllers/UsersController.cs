using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Dtos.Users;
using UserService.Data.Users;
using UserService.Dtos.Transactions;
using UserService.SyncDataService;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using UserService.KafkaHandlers;
using UserService.Constants;
using System.Text.Json;

namespace AuthService.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class UsersController : ControllerBase
  {
    private IUser _user;
    private readonly ITransactionClient _transactionClient;
    private readonly ProducerHandler _producerHandler;

    public UsersController(IUser user, ITransactionClient transactionClient, ProducerHandler producerHandler)
    {
      _user = user;
      _transactionClient = transactionClient;
      _producerHandler = producerHandler;
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
    [Authorize(Roles = "Admin")]
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

    [HttpPost("order")]
    [Authorize(Roles = "User")]
    public async Task<ActionResult<OrderOutput>> Order([FromBody] FeeInput input)
    {
      try
      {

        var result = await _user.GetUserById(input.UserId);
        string token = Request.Headers["Authorization"];
        string[] tokenWords = token.Split(' ');
        var orderFee = await _transactionClient.CheckOrderFee(input, tokenWords[1]);
        if (!orderFee.canOrder)
        {
          throw new Exception($"Saldo user id: {input.UserId} is not enough!");
        }
        //kalo benar kasihkan ke kafka
        string value = JsonSerializer.Serialize(input);
        await _producerHandler.ProduceMessage(TopicList.OrderTopic, TopicKeyList.NewOrder, value);
        return Ok(new OrderOutput { Message = "Order data sent successfully", Data = orderFee });
      }
      catch (System.Exception ex)
      {
        return BadRequest(ex);
      }
    }
  }
}