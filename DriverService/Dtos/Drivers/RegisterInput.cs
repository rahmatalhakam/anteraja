using System.ComponentModel.DataAnnotations;

namespace DriverService.Dtos.Drivers
{
    public class RegisterInput
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

    }
}
