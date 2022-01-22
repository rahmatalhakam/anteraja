using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DriverService.Dtos.Orders
{
    public class CreateTransactionInput
    {
        public string UserId { get; set; }
        public string DriverId { get; set; }
        public double LatStart { get; set; }
        public double LongStart { get; set; }
        public double LatEnd { get; set; }
        public double LongEnd { get; set; }
        public string Area { get; set; }

    }
}