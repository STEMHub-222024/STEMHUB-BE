using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using STEMHub.STEMHub_Data.Entities;

namespace STEMHub.STEMHub_Data.Data
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(256)]
        public string? FirstName { get; set; }
        [MaxLength(256)]
        public string? LastName { get; set; }
        [MaxLength(450)]
        public string? Image { get; set; }
        [MaxLength(450)]
        public string? Address { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }
        public string? PasswordResetToken { get; set; }
        public DateTime? ResetTokenExpires { get; set; }
        public string? OTPEnableTwoFactor { get; set; }
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<NewspaperArticle> NewspaperArticles { get; set; } = new List<NewspaperArticle>();
    }
}
