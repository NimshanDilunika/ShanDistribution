using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShanDistribution.Data;
using ShanDistribution.Models;
using ShanDistribution.ViewModels;

namespace ShanDistribution.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // 1. Calculate Invoices Count per Customer
            var customerInvoiceGroups = await _context.CustomerInvoices
                .GroupBy(ci => ci.CustomerId)
                .Select(g => new
                {
                    CustomerId = g.Key,
                    InvoiceCount = g.Count()
                })
                .ToListAsync();

            var allCustomers = await _context.Customers.ToListAsync();

            // Match customers with their invoice counts
            var customerPerformance = allCustomers.Select(c =>
            {
                var match = customerInvoiceGroups.FirstOrDefault(g => g.CustomerId == c.CustomerId);
                return new CustomerPerformanceItem
                {
                    CustomerId = c.CustomerId,
                    CustomerName = c.CustomerName ?? "Customer #" + c.CustomerId,
                    TotalInvoices = match?.InvoiceCount ?? 0
                };
            }).ToList();

            var model = new DashboardViewModel
            {
                // Core metric counts
                TotalActiveCustomers = allCustomers.Count,
                TotalInvoicesCount = await _context.CustomerInvoices.CountAsync(),
                ActiveProductsCount = await _context.Products.CountAsync(),
                TotalActiveSuppliers = await _context.Suppliers.CountAsync(),
                TotalBillsCount = await _context.SupplierBills.CountAsync(),

                // Low Stock Products
                LowStockProducts = await _context.Products
                    .Where(p => p.StockCount <= 20)
                    .OrderBy(p => p.StockCount)
                    .Take(5)
                    .Select(p => new LowStockProductItem
                    {
                        ProductId = p.ProductId,
                        ProductName = p.ProductName,
                        Price = p.Price,
                        StockCount = p.StockCount
                    })
                    .ToListAsync(),

                // Top Selling Products
                TopSellingProducts = await _context.CustomerInvoiceProducts
                    .GroupBy(cip => cip.ProductId)
                    .Select(g => new
                    {
                        ProductId = g.Key,
                        UnitsSold = g.Count()
                    })
                    .OrderByDescending(x => x.UnitsSold)
                    .Take(5)
                    .Join(_context.Products,
                          grouped => grouped.ProductId,
                          prod => prod.ProductId,
                          (grouped, prod) => new TopProductItem
                          {
                              ProductId = prod.ProductId,
                              ProductName = prod.ProductName,
                              TotalUnitsSold = grouped.UnitsSold,
                              TotalRevenue = grouped.UnitsSold * prod.Price
                          })
                    .ToListAsync(),

                // Top 5 Customers by Invoice Count
                TopSellingCustomers = customerPerformance
                    .OrderByDescending(c => c.TotalInvoices)
                    .Take(5)
                    .ToList(),

                // Low 5 Customers by Invoice Count
                LowSellingCustomers = customerPerformance
                    .OrderBy(c => c.TotalInvoices)
                    .Take(5)
                    .ToList()
            };

            return View(model);
        }

        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}