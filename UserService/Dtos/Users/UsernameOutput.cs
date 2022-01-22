using System.Collections.Generic;
using System.Threading.Tasks;

namespace UserService.Dtos.Users
{
    public class UsernameOutput
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public List<string> Role { get; set; }
    }
}
