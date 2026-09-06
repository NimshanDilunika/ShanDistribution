
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ShanDistribution.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string? ProfilePicture { get; set; }

    }
}