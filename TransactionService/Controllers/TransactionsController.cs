using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TransactionService.Data;
using TransactionService.Dtos;
using TransactionService.Helpers;
using TransactionService.Models;
using TransactionService.SyncDataService;

namespace TransactionService.Controllers
{


  [Route("api/v1/[controller]")]
  [ApiController]
  public class TransactionsController : ControllerBase
  {
    private readonly IMapper _mapper;
    private readonly ITransaction _transaction;
    private readonly IUserDataClient _userClient;
    private readonly IDriverDataClient _driverClient;
    private readonly IWalletMutation _walletMutation;

    private readonly IWalletUser _walletUser;

    public TransactionsController(IWalletUser walletUser,
                                  IMapper mapper,
                                  IWalletMutation walletMutation,
                                  IUserDataClient userClient,
                                  IDriverDataClient driverClient,
                                  Data.ITransaction transaction)
    {
      _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
      _transaction = transaction;
      _userClient = userClient;
      _driverClient = driverClient;
      _walletMutation = walletMutation;
      _walletUser = walletUser;
    }


    [HttpPost]
    [Authorize(Roles = "Driver")]
    public async Task<ActionResult<TransactionOutput>> Post([FromBody] TransactionInput input)
    {
      try
      {
        if (!await _userClient.GetById(input.UserId))
          throw new Exception($"User id: {input.UserId} is not found");
        if (!await _driverClient.GetById(input.DriverId))
          throw new Exception($"Driver id: {input.DriverId} is not found");
        var model = _mapper.Map<Transaction>(input);
        int distance = Convert.ToInt32(Coordinate.DistanceTo(input.LatStart, input.LongStart, input.LatEnd, input.LongEnd));
        if (distance > 30)
        {
          return BadRequest("Maximum distance is 30 Km");
        }
        if (input.Area == null)
          input.Area = "BASE";
        var result = await _transaction.Insert(input.Area, distance, model);
        var userWallet = await _walletUser.GetByCustomerId(input.UserId);
        if (userWallet == null)
          throw new System.Exception($"Customer id: {input.UserId} is not found.");
        var withdraw = await Withdraw(new WithdrawInput { Debit = result.Billing, WalletUserId = userWallet.Id });
        return Ok(_mapper.Map<TransactionOutput>(result));
      }
      catch (System.Exception ex)
      {
        return BadRequest(ex);
      }
    }

    [HttpGet("customer/{id}")]
    [Authorize(Roles = "Driver,User")]
    public async Task<ActionResult<IEnumerable<TransactionOutput>>> GetById(string id)
    {
      try
      {
        //TODO: benerin lagi
        string roleName = User.FindFirst(ClaimTypes.Role)?.Value;
        if (roleName == "Driver" && !await _driverClient.GetById(id))
          throw new Exception($"Driver id: {id} is not found");
        if (roleName == "User" && !await _userClient.GetById(id))
          throw new Exception($"User id: {id} is not found");
        var result = await _transaction.GetByCustomerId(id, roleName);
        return Ok(_mapper.Map<IEnumerable<TransactionOutput>>(result));
      }
      catch (System.Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<TransactionOutput>>> GetAll()
    {
      try
      {
        return Ok(_mapper.Map<IEnumerable<TransactionOutput>>(await _transaction.GetAll()));
      }
      catch (System.Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    [HttpPut("finish")]
    [Authorize(Roles = "Driver")]
    public async Task<ActionResult<TransactionOutput>> FinishedTransaction([FromBody] FinishTransInput input)
    {
      try
      {
        if (!await _driverClient.GetById(input.DriverId))
          throw new Exception($"Driver id: {input.DriverId} is not found");
        var result = await _transaction.UpdateStatus(input.TransactionId, input.DriverId);
        var driverWallet = await _walletUser.GetByCustomerId(result.DriverId);
        if (driverWallet == null)
          throw new System.Exception($"Customer id: {result.DriverId} is not found.");
        var topup = await Topup(new TopUpInput { Credit = result.Billing, WalletUserId = driverWallet.Id });
        return Ok(_mapper.Map<TransactionOutput>(result));
      }
      catch (System.Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }


    private async Task<MutationOutput> Withdraw(WithdrawInput input)
    {
      try
      {
        var walletUser = await _walletUser.GetById(input.WalletUserId);
        if (walletUser == null)
          throw new Exception($"Wallet user id: {input.WalletUserId} not found");
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
        return _mapper.Map<MutationOutput>(result);
      }
      catch (System.Exception ex)
      {
        throw new Exception(ex.Message);
      }
    }

    private async Task<MutationOutput> Topup(TopUpInput input)
    {
      try
      {
        var walletUser = await _walletUser.GetById(input.WalletUserId);
        if (walletUser == null)
          throw new Exception($"Wallet user id: {input.WalletUserId} not found");
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
        return _mapper.Map<MutationOutput>(result);
      }
      catch (System.Exception ex)
      {
        throw new Exception(ex.Message);
      }
    }

    // [HttpPost("fee")]
    // [Authorize(Roles = "USER")]
    // public async Task<ActionResult<FeeOutput>> TransactionFee([FromBody] FeeInput input)
    // {
    //   try
    //   {
    // int distance = Convert.ToInt32(Coordinate.DistanceTo(input.LatStart, input.LongStart, input.LatEnd, input.LongEnd));
    // if (distance > 30)
    // {
    //   return BadRequest("Maximum distance is 30 Km");
    // }
    // if (input.Area == null)
    //   input.Area = "BASE";
    // var result = await _transaction.GetFeeByUserId(input.Area, distance);
    // return Ok(result);
    //   return Ok();
    // }
    // catch (System.Exception ex)
    // {
    //   return BadRequest(ex.Message);
    // }
    // }





  }
}