using System.ComponentModel.DataAnnotations;

namespace ShanDistribution.Models
{
    public class Customer
    {
        [Key]
        [Display(Name = "Customer ID")]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Customer Name is required.")]
        [StringLength(100)]
        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(200)]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Phone Number is required.")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        [Display(Name = "Phone No")]
        public string PhoneNo { get; set; }
    }
}