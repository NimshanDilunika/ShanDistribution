using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShanDistribution.ViewModels
{
    public class CustomerInvoiceItemDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
        public decimal TotalPrice { get; set; }
    }

    public class CustomerInvoiceCreateDto
    {
        [Required]
        public string InvoiceNo { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public string CustomerName { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public decimal Amount { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Payment { get; set; }

   

        public List<CustomerInvoiceItemDto> Items { get; set; } = new List<CustomerInvoiceItemDto>();
    }

    public class MakeCustomerPaymentDto
    {
        [Required]
        public int CIId { get; set; }

        public string InvoiceNo { get; set; }
        public string CustomerName { get; set; }
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