﻿using System.ComponentModel.DataAnnotations;

namespace DriverService.Dtos.Users
{
    public class RegisterInput
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
    }
}