using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DriverService.Dtos.Drivers
{
    public class LockUserInput
    {
        public string Username { get; set; }
        public bool isLock { get; set; }
    }
}