using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShanDistribution.Models;

namespace ShanDistribution.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<SupplierBill> SupplierBills { get; set; }
        public DbSet<SupplierBillProduct> SupplierBillProducts { get; set; }
        public DbSet<SupplierBillPayment> SupplierBillPayments { get; set; }
        public DbSet<CustomerInvoice> CustomerInvoices { get; set; }
        public DbSet<CustomerInvoiceProduct> CustomerInvoiceProducts { get; set; }
        public DbSet<CustomerInvoicePayment> CustomerInvoicePayments { get; set; }
    }
}