using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DriverService.Dtos.DriverProfiles
{
    public class SetLongLatInput
    {
        [Required]
        public double LongNow { get; set; }
        [Required]
        public double LatNow { get; set; }
    }
}