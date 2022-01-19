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

    public TransactionsController(Data.ITransaction transaction, IMapper mapper, IUserDataClient userClient, IDriverDataClient driverClient)
    {
      _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
      _transaction = transaction;
      _userClient = userClient;
      _driverClient = driverClient;
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
        return Ok(_mapper.Map<TransactionOutput>(result));
      }
      catch (System.Exception ex)
      {
        return BadRequest(ex.Message);
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
    // [Authorize(Roles = "DRIVER")]
    public async Task<ActionResult<TransactionOutput>> FinishedTransaction([FromBody] FinishTransInput input)
    {
      try
      {
        //cek driver id dengan post ke microesrvice lainnya
        var result = await _transaction.UpdateStatus(input.TransactionId, input.DriverId);
        return Ok(_mapper.Map<TransactionOutput>(result));
      }
      catch (System.Exception ex)
      {
        return BadRequest(ex.Message);
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