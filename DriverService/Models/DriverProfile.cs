using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DriverService.Models
{
    public class DriverProfile
    {

        public string DriverId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public double LongNow { get; set; }
        public double LatNow { get; set; }

        public User Users { get; set; }
    }
}