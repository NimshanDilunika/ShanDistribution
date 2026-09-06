using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShanDistribution.Models
{
    public class CustomerInvoicePayment
    {
        [Key]
        [Display(Name = "Invoice Payment ID")]
        public int InvoicePaymentId { get; set; }

        [Required]
        [Display(Name = "Invoice ID")]
        public int CIId { get; set; }

        [ForeignKey("CIId")]
        public virtual CustomerInvoice CustomerInvoice { get; set; }

        [Required(ErrorMessage = "Payment amount is required.")]
        [Range(0.01, 10000000.00, ErrorMessage = "Payment must be greater than 0.")]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Payment")]
        public decimal Payment { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Payment Date")]
        public DateTime Date { get; set; }
    }
}