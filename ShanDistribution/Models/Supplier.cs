using System.ComponentModel.DataAnnotations;

namespace ShanDistribution.Models
{
    public class Supplier
    {
        [Key]
        [Display(Name = "Supplier ID")]
        public int SupplierId { get; set; }

        [Required(ErrorMessage = "Supplier Name is required.")]
        [StringLength(100)]
        [Display(Name = "Supplier Name")]
        public string SupplierName { get; set; }

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