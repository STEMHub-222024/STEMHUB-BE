using System.ComponentModel.DataAnnotations;

namespace STEMHub.STEMHub_Service.Authentication.SignUp
{
    public class RegisterUser
    {
        [Required(ErrorMessage = "User Name is required")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "User Name is required")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "User Name is required")]
        public string? LastName { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }

        public List<string>? Roles { get; set; }
    }
}
