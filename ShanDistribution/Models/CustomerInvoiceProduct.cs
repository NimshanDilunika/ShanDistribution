using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShanDistribution.Models
{
    public class CustomerInvoiceProduct
    {
        [Key]
        [Display(Name = "CIP ID")]
        public int CIPId { get; set; }

        [Required]
        [Display(Name = "CI ID")]
        public int CIId { get; set; }

        [ForeignKey("CIId")]
        public virtual CustomerInvoice CustomerInvoice { get; set; }

        [Required]
        [Display(Name = "Product ID")]
        public int ProductId { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Price")]
        public decimal Price { get; set; }

        [Required]
        [Range(1, 100000)]
        [Display(Name = "Count")]
        public int Count { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Total Price")]
        public decimal TotalPrice { get; set; }
    }
}