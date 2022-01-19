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

namespace TransactionService.Controllers
{

  [ApiController]
  [Route("api/v1/[controller]")]
  public class WalletsController : ControllerBase
  {
    private readonly IMapper _mapper;
    private readonly IWalletUser _walletUser;
    private readonly IWalletMutation _walletMutation;

    public WalletsController(IWalletUser walletUser, IMapper mapper, IWalletMutation walletMutation)
    {
      _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
      _walletUser = walletUser;
      _walletMutation = walletMutation;
    }

    [HttpPost("Users")]
    // [Authorize(Roles = "USER,DRIVER")]
    public async Task<ActionResult<WalletUserOutput>> UserPost([FromBody] WalletUserInput input)
    {
      try
      {
        // TODO: check user id beneran ada atau tidak
        string roleName = User.FindFirst(ClaimTypes.Role)?.Value;
        var walletUserObj = new WalletUser { CustomerId = input.CustomerId, Rolename = roleName, CreatedAt = DateTime.Now };
        var result = await _walletUser.Insert(walletUserObj);
        return Ok(_mapper.Map<WalletUserOutput>(result));
      }
      catch (System.Exception ex)
      {
        return BadRequest(ex);
      }
    }

    [HttpPost("Mutations/TopUp")]
    // [Authorize(Roles = "USER,DRIVER")]
    public async Task<ActionResult<MutationOutput>> Topup([FromBody] TopUpInput input)
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
        return Ok(result);
      }
      catch (System.Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    [HttpPost("Mutations/Withdraw")]
    public async Task<ActionResult<MutationOutput>> Withdraw([FromBody] WithdrawInput input)
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
        return Ok(result);
      }
      catch (System.Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }
  }
}