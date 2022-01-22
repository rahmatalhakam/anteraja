using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DriverService.Dtos.Orders
{
    public class AcceptOrderInput
    {
        public string DriverId { get; set; }
        public int OrderId { get; set; }
        public double LongNow { get; set; }
        public double LatNow { get; set; }
    }
}