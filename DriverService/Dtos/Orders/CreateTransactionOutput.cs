using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DriverService.Dtos.Orders
{
    public class CreateTransactionOutput
    {
        public int id { get; set; }
        public string userId { get; set; }
        public string driverId { get; set; }

        public double latStart { get; set; }

        public double longStart { get; set; }

        public double latEnd { get; set; }

        public double longEnd { get; set; }

        public DateTime createdAt { get; set; }
        public string status { get; set; }
        public int distance { get; set; }
        public float billing { get; set; }
    }
}