﻿using System.ComponentModel.DataAnnotations;

namespace STEMHub.STEMHub_Services.Authentication.Login
{
    public class ResetPasswordRequest
    {
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Token { get; set; } = string.Empty;
        [Required, MinLength(6, ErrorMessage = "Please enter at least 6 characters, dude!")]
        public string Password { get; set; } = string.Empty;
        [Required, Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}