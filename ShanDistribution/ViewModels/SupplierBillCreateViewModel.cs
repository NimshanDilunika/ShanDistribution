using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShanDistribution.ViewModels
{
    public class SupplierBillItemDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
        public decimal TotalPrice { get; set; }
    }

    public class SupplierBillCreateDto
    {
        [Required]
        public string BillNo { get; set; }

        [Required]
        public int SupplierId { get; set; }

        [Required]
        public string SupplierName { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public decimal Amount { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Payment { get; set; } // Initial payment input

        public List<SupplierBillItemDto> Items { get; set; } = new List<SupplierBillItemDto>();
    }

    public class MakeSupplierPaymentDto
    {
        [Required]
        public int SBId { get; set; }

        public string BillNo { get; set; }
        public string SupplierName { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal BalanceAmount { get; set; }

        [Required(ErrorMessage = "Please enter a payment amount.")]
        [Range(0.01, 10000000.00, ErrorMessage = "Payment must be greater than 0.")]
        public decimal Payment { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; } = DateTime.Now;
    }
}