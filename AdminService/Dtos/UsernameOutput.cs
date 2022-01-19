using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminService.Dtos
{
    public class UsernameOutput
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public List<string> Role { get; set; }
    }
}