using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace DriverService.Profiles
{
    public class DriversProfile : Profile
    {
        public DriversProfile()
        {
            CreateMap<Models.DriverProfile, Dtos.DriverProfiles.DriverProfileDto>()
              .ForMember(dest => dest.Name,
              opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));

            CreateMap<Dtos.DriverProfiles.DriverProfileInput, Models.DriverProfile>();
            CreateMap<Dtos.DriverProfiles.SetLongLatInput, Models.DriverProfile>();
            CreateMap<Dtos.DriverProfiles.UpdateDriverProfileInput, Models.DriverProfile>();
        }
    }
}