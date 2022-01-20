using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DriverService.Dtos.DriverProfiles;
using DriverService.Models;

namespace DriverService.Data.DriverProfiles
{
    public interface IDriverProfile : ICrud<DriverProfile>
    {
        Task<DriverProfile> GetProfileById();
        Task<DriverProfile> SetPosition(DriverProfile input);

    }
}