﻿using System.ComponentModel.DataAnnotations;

namespace STEMHub.STEMHub_Services.Authentication.Login
{
    public class Login
    {
        [Required(ErrorMessage = "User Name is required")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
    }
}