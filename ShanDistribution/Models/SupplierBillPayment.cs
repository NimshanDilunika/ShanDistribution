using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShanDistribution.Models
{
    public class SupplierBillPayment
    {
        [Key]
        [Display(Name = "Bill Payment ID")]
        public int BillPaymentId { get; set; }

        [Required]
        [Display(Name = "Bill ID")]
        public int SBId { get; set; }

        [ForeignKey("SBId")]
        public virtual SupplierBill SupplierBill { get; set; }

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