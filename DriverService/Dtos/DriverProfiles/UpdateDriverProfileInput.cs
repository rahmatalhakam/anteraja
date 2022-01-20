using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DriverService.Dtos.DriverProfiles
{
    public class UpdateDriverProfileInput
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public double LongNow { get; set; }
        public double LatNow { get; set; }
    }
}