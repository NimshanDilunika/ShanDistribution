using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShanDistribution.Models
{
    public class CustomerInvoice
    {
        [Key]
        [Display(Name = "CI ID")]
        public int CIId { get; set; }

        [Required(ErrorMessage = "Invoice No is required.")]
        [StringLength(50)]
        [Display(Name = "Invoice No")]
        public string InvoiceNo { get; set; }

        [Required]
        [Display(Name = "Customer ID")]
        public int CustomerId { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }

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

        // PAYMENT TRACKING COLUMNS
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Paid Amount")]
        public decimal PaidAmount { get; set; } = 0.00m;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Balance Amount")]
        public decimal BalanceAmount { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Status")]
        public string Status { get; set; } = "Initial";

        public virtual ICollection<CustomerInvoiceProduct> InvoiceProducts { get; set; } = new List<CustomerInvoiceProduct>();
        public virtual ICollection<CustomerInvoicePayment> InvoicePayments { get; set; } = new List<CustomerInvoicePayment>();
    }
}