using System.ComponentModel.DataAnnotations;

namespace UserService.Dtos.Users
{
    public class LoginInput
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
