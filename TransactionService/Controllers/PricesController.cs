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
namespace TransactionService.Controllers
{
  [Route("api/v1/[controller]")]
  [ApiController]
  public class PricesController : ControllerBase
  {
    private readonly IMapper _mapper;
    private readonly IPrice _price;
    public PricesController(IPrice price, IMapper mapper)
    {
      _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
      _price = price;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<Price>>> GetPrices()
    {
      try
      {
        var results = await _price.GetAll();
        return Ok(results);
      }
      catch (System.Exception e)
      {
        return BadRequest(e.Message);
      }
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Price>> Post([FromBody] PriceInput input)
    {
      try
      {
        var dto = _mapper.Map<Price>(input);
        var result = await _price.Insert(dto);
        return Ok(result);
      }
      catch (System.Exception e)
      {
        return BadRequest(e.Message);
      }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Price>> Put(int id, [FromBody] PriceInput input)
    {
      try
      {
        var dto = _mapper.Map<Price>(input);
        var result = await _price.Update(id, dto);
        return Ok(result);
      }
      catch (System.Exception e)
      {
        return BadRequest(e.Message);
      }
    }
  }
}