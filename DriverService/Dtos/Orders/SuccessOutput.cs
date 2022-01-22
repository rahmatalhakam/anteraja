using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DriverService.Dtos.Orders
{
    public class SuccessOutput
    {
        public string Message { get; set; }
        public CreateTransactionOutput Data { get; set; }
    }
}