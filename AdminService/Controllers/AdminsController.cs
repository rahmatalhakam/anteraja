using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminService.Data;
using AdminService.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AdminService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminsController : ControllerBase
    {
        private IAdmin _admin;
        private IMapper _mapper;

        public AdminsController(IAdmin admin, IMapper mapper)
        {
            _admin = admin ?? throw new ArgumentNullException(nameof(admin));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        // GET api/<AdminsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Admin>> Get(int id)
        {
            var result = await _admin.GetAdminById(id.ToString());
            if (result == null)
                return NotFound();

            return Ok(_mapper.Map<Admin>(result));
        }

        // PUT api/<AdminsController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Admin>> Put(int id, [FromBody] Admin admin)
        {
            try
            {
                var adm = _mapper.Map<Admin>(admin);
                var result = await _admin.Update(id.ToString(), adm);
                return Ok(_mapper.Map<Admin>(result));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<AdminsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _admin.Delete(id.ToString());
                return Ok($"delete data admin id {id} berhasil");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}