using System.ComponentModel.DataAnnotations;

namespace DriverService.Dtos.Users
{
    public class RegisterInput
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string NoKTP { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

    }
}
