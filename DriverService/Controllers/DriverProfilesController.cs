using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DriverService.Data.DriverProfiles;
using DriverService.Dtos.DriverProfiles;
using DriverService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DriverService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DriverProfilesController : ControllerBase
    {
        private readonly IDriverProfile _driverProfile;
        private readonly IMapper _mapper;

        public DriverProfilesController(IDriverProfile driverProfile, IMapper mapper)
        {
            _driverProfile = driverProfile;
            _mapper = mapper;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DriverProfileDto>>> Get()
        {
            var profiles = await _driverProfile.GetAll();

            var dtos = _mapper.Map<IEnumerable<DriverProfileDto>>(profiles);
            return Ok(dtos);
        }

        [HttpPost]
        public async Task<ActionResult<DriverProfileDto>> Post([FromBody] DriverProfileInput input)
        {
            try
            {
                var data = _mapper.Map<Models.DriverProfile>(input);
                var result = await _driverProfile.Insert(data);
                var profileReturn = _mapper.Map<Dtos.DriverProfiles.DriverProfileDto>(result);
                return Ok(profileReturn);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetProfile")]
        public async Task<ActionResult<DriverProfileDto>> GetProfileById()
        {
            var result = await _driverProfile.GetProfileById();
            if (result == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<DriverProfileDto>(result));
        }
        [HttpPost("Position")]
        public async Task<ActionResult<DriverProfileDto>> SetPosition([FromBody] SetLongLatInput input)
        {
            try
            {
                var data = _mapper.Map<Models.DriverProfile>(input);
                var result = await _driverProfile.SetPosition(data);
                var profileReturn = _mapper.Map<Dtos.DriverProfiles.DriverProfileDto>(result);
                return Ok(profileReturn);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        public async Task<ActionResult<DriverProfileDto>> Update([FromBody] UpdateDriverProfileInput input)
        {
            try
            {
                var profile = _mapper.Map<DriverProfile>(input);
                var result = await _driverProfile.Update(profile);
                var dto = _mapper.Map<DriverProfileDto>(result);

                // return Ok(dto);
                return Ok(new { Message = "Update Success", data = dto });
            }
            catch (System.Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}