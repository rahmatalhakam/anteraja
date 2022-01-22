using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DriverService.Dtos.Orders
{
    public class DistanceInput
    {
        public double lat1 { get; set; }
        public double long1 { get; set; }
        public double lat2 { get; set; }
        public double long2 { get; set; }
    }
}