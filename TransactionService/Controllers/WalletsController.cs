using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TransactionService.Data;
using TransactionService.Dtos;
using TransactionService.Models;
using TransactionService.SyncDataService;

namespace TransactionService.Controllers
{

  [ApiController]
  [Route("api/v1/[controller]")]
  public class WalletsController : ControllerBase
  {
    private readonly IMapper _mapper;
    private readonly IWalletUser _walletUser;
    private readonly IWalletMutation _walletMutation;
    private readonly IUserDataClient _userClient;
    private readonly IDriverDataClient _driverClient;

    public WalletsController(IWalletUser walletUser, IMapper mapper, IWalletMutation walletMutation, IUserDataClient userClient, IDriverDataClient driverClient)
    {
      _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
      _walletUser = walletUser;
      _walletMutation = walletMutation;
      _userClient = userClient;
      _driverClient = driverClient;
    }

    [HttpPost("Users")]
    [Authorize(Roles = "User,Driver")]
    public async Task<ActionResult<WalletUserOutput>> UserPost([FromBody] WalletUserInput input)
    {
      try
      {
        string roleName = User.FindFirst(ClaimTypes.Role)?.Value;
        if (roleName == "Driver" && !await _driverClient.GetById(input.CustomerId))
          throw new Exception($"Driver id: {input.CustomerId} is not found");
        if (roleName == "User" && !await _userClient.GetById(input.CustomerId))
          throw new Exception($"User id: {input.CustomerId} is not found");
        var walletUserObj = new WalletUser { CustomerId = input.CustomerId, Rolename = roleName, CreatedAt = DateTime.Now };
        var result = await _walletUser.Insert(walletUserObj);
        return Ok(_mapper.Map<WalletUserOutput>(result));
      }
      catch (System.Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    [HttpPost("Mutations/TopUp")]
    [Authorize(Roles = "User,Driver")]
    public async Task<ActionResult<MutationOutput>> Topup([FromBody] TopUpInput input)
    {
      try
      {
        string roleName = User.FindFirst(ClaimTypes.Role)?.Value;
        var walletUser = await _walletUser.GetById(input.WalletUserId);
        if (walletUser == null)
          throw new Exception($"Wallet user id: {input.WalletUserId} not found");
        if (walletUser.Rolename != roleName)
          return Forbid();
        var dto = _mapper.Map<WalletMutation>(input);
        dto.CreatedAt = DateTime.Now;
        dto.WalletUser = walletUser;
        var walletMutation = await _walletMutation.GetByWalletUserId(input.WalletUserId);
        if (walletMutation == null)
        {
          dto.Saldo = input.Credit;
        }
        else
        {
          dto.Saldo = walletMutation.Saldo + input.Credit;
        }
        var result = await _walletMutation.Insert(dto);
        return Ok(_mapper.Map<MutationOutput>(result));
      }
      catch (System.Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    [HttpPost("Mutations/Withdraw")]
    [Authorize(Roles = "User,Driver")]
    public async Task<ActionResult<MutationOutput>> Withdraw([FromBody] WithdrawInput input)
    {
      try
      {
        string roleName = User.FindFirst(ClaimTypes.Role)?.Value;
        var walletUser = await _walletUser.GetById(input.WalletUserId);
        if (walletUser == null)
          throw new Exception($"Wallet user id: {input.WalletUserId} not found");
        if (walletUser.Rolename != roleName)
          return Forbid();
        var dto = _mapper.Map<WalletMutation>(input);
        dto.CreatedAt = DateTime.Now;
        dto.WalletUser = walletUser;
        var walletMutation = await _walletMutation.GetByWalletUserId(input.WalletUserId);
        if (walletMutation == null)
        {
          throw new Exception($"Saldo is not enough.");
        }
        if (walletMutation.Saldo < input.Debit)
        {
          throw new Exception($"Saldo is not enough.");
        }
        dto.Saldo = walletMutation.Saldo - input.Debit;
        var result = await _walletMutation.Insert(dto);
        return Ok(_mapper.Map<MutationOutput>(result));
      }
      catch (System.Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    [HttpGet("Mutations/Saldo/{id}")]
    [Authorize(Roles = "User,Driver")]
    public async Task<ActionResult<MutationOutput>> GetSaldo(int id)
    {
      try
      {
        // TODO: cek lagi ketika sudah ada mutasinya dan lebih dari 1
        string roleName = User.FindFirst(ClaimTypes.Role)?.Value;
        var walletUser = await _walletUser.GetById(id);
        if (walletUser == null)
          throw new Exception($"Wallet user id: {id} not found");
        if (walletUser.Rolename != roleName)
          return Forbid();
        var walletMutation = await _walletMutation.GetByWalletUserId(id);
        if (walletMutation == null) return BadRequest($"Cannot get saldo, no mutations found. Topup first!");
        return Ok(_mapper.Map<MutationOutput>(walletMutation));
      }
      catch (System.Exception ex)
      {
        return BadRequest(ex);
      }
    }
    [HttpGet("Users/Customers/{id}")]
    [Authorize(Roles = "User,Driver")]
    public async Task<ActionResult<WalletUserOutput>> GetByCustomerId(string id)
    {
      try
      {
        var result = await _walletUser.GetByCustomerId(id);
        if (result == null)
          throw new System.Exception($"Customer id: {id} is not found.");
        return Ok(_mapper.Map<WalletUserOutput>(result));
      }
      catch (System.Exception ex)
      {
        return BadRequest(ex);
      }
    }
  }
}