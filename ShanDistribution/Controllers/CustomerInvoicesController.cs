using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShanDistribution.Data;
using ShanDistribution.Models;
using ShanDistribution.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ShanDistribution.Controllers
{
    [Authorize(Roles = "Admin,Manager,Rep")]
    public class CustomerInvoicesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomerInvoicesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CustomerInvoices
        public async Task<IActionResult> Index()
        {
            var invoices = await _context.CustomerInvoices
                .OrderByDescending(i => i.Date)
                .ThenByDescending(i => i.CIId)
                .ToListAsync();
            return View(invoices);
        }

        // GET: CustomerInvoices/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Customers = await _context.Customers.OrderBy(c => c.CustomerName).ToListAsync();
            ViewBag.Products = await _context.Products.OrderBy(p => p.ProductName).ToListAsync();
            return View();
        }

        // POST: CustomerInvoices/Create
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CustomerInvoiceCreateDto model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.InvoiceNo))
            {
                return BadRequest(new { success = false, message = "Please enter an Invoice No." });
            }

            if (model.Items == null || !model.Items.Any())
            {
                return BadRequest(new { success = false, message = "Please add at least one product to the invoice." });
            }

            if (model.CustomerId <= 0)
            {
                return BadRequest(new { success = false, message = "Please select a valid customer." });
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1. Stock Validation
                foreach (var item in model.Items)
                {
                    var productInDb = await _context.Products.FindAsync(item.ProductId);
                    if (productInDb == null)
                    {
                        return BadRequest(new { success = false, message = $"Product '{item.ProductName}' not found." });
                    }

                    if (productInDb.StockCount < item.Count)
                    {
                        return BadRequest(new { success = false, message = $"Insufficient stock for '{productInDb.ProductName}'. Available: {productInDb.StockCount}, Requested: {item.Count}." });
                    }
                }

                // 2. Financial Balance & Status Calculation
                decimal initialPayment = model.Payment > 0 ? model.Payment : 0.00m;
                decimal balance = model.TotalAmount - initialPayment;

                string status;
                if (initialPayment <= 0)
                {
                    status = "Initial";
                }
                else if (balance <= 0)
                {
                    status = "Complete";
                }
                else
                {
                    status = "Balance";
                }

                // 3. Save Invoice Header
                var invoice = new CustomerInvoice
                {
                    InvoiceNo = model.InvoiceNo,
                    CustomerId = model.CustomerId,
                    CustomerName = model.CustomerName,
                    Date = model.Date,
                    Amount = model.Amount,
                    Discount = model.Discount,
                    TotalAmount = model.TotalAmount,
                    PaidAmount = initialPayment,
                    BalanceAmount = balance,
                    Status = status
                };

                _context.CustomerInvoices.Add(invoice);
                await _context.SaveChangesAsync();

                // 4. Save Invoice Products & Deduct Stock
                foreach (var item in model.Items)
                {
                    var invoiceProduct = new CustomerInvoiceProduct
                    {
                        CIId = invoice.CIId,
                        ProductId = item.ProductId,
                        ProductName = item.ProductName,
                        Price = item.Price,
                        Count = item.Count,
                        TotalPrice = item.TotalPrice
                    };

                    _context.CustomerInvoiceProducts.Add(invoiceProduct);

                    var productInDb = await _context.Products.FindAsync(item.ProductId);
                    if (productInDb != null)
                    {
                        productInDb.StockCount -= item.Count;
                        productInDb.ItemCount -= item.Count;
                        _context.Products.Update(productInDb);
                    }
                }

                // 5. Record Initial Payment
                if (initialPayment > 0)
                {
                    var invoicePayment = new CustomerInvoicePayment
                    {
                        CIId = invoice.CIId,
                        Payment = initialPayment,
                        Date = model.Date
                    };
                    _context.CustomerInvoicePayments.Add(invoicePayment);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Json(new { success = true, redirectUrl = Url.Action(nameof(Index)) });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new { success = false, message = "Error saving invoice: " + ex.Message });
            }
        }

        // GET: CustomerInvoices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var invoice = await _context.CustomerInvoices
                .Include(i => i.InvoiceProducts)
                .Include(i => i.InvoicePayments)
                .FirstOrDefaultAsync(m => m.CIId == id);

            if (invoice == null) return NotFound();

            return View(invoice);
        }

        // GET: CustomerInvoices/Payment/5
        public async Task<IActionResult> Payment(int? id)
        {
            if (id == null) return NotFound();

            var invoice = await _context.CustomerInvoices.FindAsync(id);
            if (invoice == null) return NotFound();

            decimal currentBalance = invoice.BalanceAmount;
            if (currentBalance == 0 && invoice.TotalAmount > invoice.PaidAmount)
            {
                currentBalance = invoice.TotalAmount - invoice.PaidAmount;
            }

            var viewModel = new MakeCustomerPaymentDto
            {
                CIId = invoice.CIId,
                InvoiceNo = invoice.InvoiceNo,
                CustomerName = invoice.CustomerName,
                TotalAmount = invoice.TotalAmount,
                PaidAmount = invoice.PaidAmount,
                BalanceAmount = currentBalance,
                Date = DateTime.Now
            };

            return View(viewModel);
        }

        // POST: CustomerInvoices/Payment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Payment(MakeCustomerPaymentDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var invoice = await _context.CustomerInvoices.FindAsync(model.CIId);
            if (invoice == null) return NotFound();

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var paymentRecord = new CustomerInvoicePayment
                {
                    CIId = invoice.CIId,
                    Payment = model.Payment,
                    Date = model.Date
                };
                _context.CustomerInvoicePayments.Add(paymentRecord);

                invoice.PaidAmount += model.Payment;
                invoice.BalanceAmount = invoice.TotalAmount - invoice.PaidAmount;

                if (invoice.BalanceAmount <= 0)
                {
                    invoice.Status = "Complete";
                }
                else
                {
                    invoice.Status = "Balance";
                }

                _context.CustomerInvoices.Update(invoice);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return RedirectToAction(nameof(Details), new { id = invoice.CIId });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                ModelState.AddModelError(string.Empty, "Error saving payment: " + ex.Message);
                return View(model);
            }
        }
    }
}