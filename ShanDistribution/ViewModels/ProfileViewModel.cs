using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ShanDistribution.ViewModels
{
    public class ProfileViewModel
    {
        public string? Email { get; set; }

        public string? Role { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        [Phone]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        public string? ExistingProfilePicture { get; set; }

        [Display(Name = "Profile Picture")]
        public IFormFile? ProfilePictureFile { get; set; }
    }
}