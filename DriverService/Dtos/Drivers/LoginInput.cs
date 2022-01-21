using System.ComponentModel.DataAnnotations;

namespace DriverService.Dtos.Drivers
{
    public class LoginInput
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
