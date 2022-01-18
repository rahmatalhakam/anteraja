using System.ComponentModel.DataAnnotations;

namespace DriverService.Dtos.Users
{
    public class LoginInput
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
