using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShanDistribution.Models
{
    public class SupplierBill
    {
        [Key]
        [Display(Name = "SB ID")]
        public int SBId { get; set; }

        [Required(ErrorMessage = "Bill No is required.")]
        [StringLength(50)]
        [Display(Name = "Bill No")]
        public string BillNo { get; set; }

        [Required]
        [Display(Name = "Supplier ID")]
        public int SupplierId { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Supplier Name")]
        public string SupplierName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date")]
        public DateTime Date { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Amount")]
        public decimal Amount { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Discount (%)")]
        public decimal Discount { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Total Amount")]
        public decimal TotalAmount { get; set; }

        // NEW PAYMENT COLUMNS
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Paid Amount")]
        public decimal PaidAmount { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Balance Amount")]
        public decimal BalanceAmount { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Status")]
        public string Status { get; set; } = "Initial";

        public virtual ICollection<SupplierBillProduct> BillProducts { get; set; } = new List<SupplierBillProduct>();
        public virtual ICollection<SupplierBillPayment> BillPayments { get; set; } = new List<SupplierBillPayment>();
    }
}