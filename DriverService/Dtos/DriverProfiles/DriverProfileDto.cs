using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DriverService.Dtos.DriverProfiles
{
    public class DriverProfileDto
    {
        public string DriverId { get; set; }

        public string Name { get; set; }
        public double LongNow { get; set; }
        public double LatNow { get; set; }
    }
}