using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShanDistribution.Models
{
    public class Product
    {
        [Key]
        [Display(Name = "Product ID")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Product Name is required.")]
        [StringLength(100)]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Product Description is required.")]
        [StringLength(500)]
        [Display(Name = "Description")]
        public string ProductDescription { get; set; }

        [Required(ErrorMessage = "Item Count is required.")]
        [Range(0, 100000, ErrorMessage = "Item count must be a non-negative number.")]
        [Display(Name = "Item Count")]
        public int ItemCount { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, 1000000.00, ErrorMessage = "Price must be greater than 0.")]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Price")]
        public decimal Price { get; set; }


        [Range(0, 100000, ErrorMessage = "Stock count must be a non-negative number.")]
        [Display(Name = "Stock Count")]
        public int StockCount { get; set; }
    }
}