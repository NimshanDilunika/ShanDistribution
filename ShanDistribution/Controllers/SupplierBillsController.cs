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
    [Authorize(Roles = "Admin,Manager")]
    public class SupplierBillsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SupplierBillsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SupplierBills
        public async Task<IActionResult> Index()
        {
            var bills = await _context.SupplierBills
                .OrderByDescending(b => b.Date)
                .ThenByDescending(b => b.SBId)
                .ToListAsync();
            return View(bills);
        }

        // GET: SupplierBills/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Suppliers = await _context.Suppliers.OrderBy(s => s.SupplierName).ToListAsync();
            ViewBag.Products = await _context.Products.OrderBy(p => p.ProductName).ToListAsync();
            return View();
        }

        // POST: SupplierBills/Create
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SupplierBillCreateDto model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.BillNo))
            {
                return BadRequest(new { success = false, message = "Please enter a valid Bill No." });
            }

            if (model.Items == null || !model.Items.Any())
            {
                return BadRequest(new { success = false, message = "Please add at least one product to the bill." });
            }

            if (model.SupplierId <= 0)
            {
                return BadRequest(new { success = false, message = "Please select a valid supplier." });
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1. Calculate Initial Payment and Initial Balance
                decimal initialPayment = model.Payment > 0 ? model.Payment : 0.00m;

                // When Payment is 0, Balance Amount EXACTLY equals Total Amount
                decimal balanceAmount = model.TotalAmount - initialPayment;

                // 2. Status determination
                string status;
                if (initialPayment <= 0)
                {
                    status = "Initial";
                }
                else if (balanceAmount <= 0)
                {
                    status = "Complete";
                }
                else
                {
                    status = "Balance";
                }

                // 3. Create Bill Entity
                var bill = new SupplierBill
                {
                    BillNo = model.BillNo,
                    SupplierId = model.SupplierId,
                    SupplierName = model.SupplierName,
                    Date = model.Date,
                    Amount = model.Amount,
                    Discount = model.Discount,
                    TotalAmount = model.TotalAmount,
                    PaidAmount = initialPayment,
                    BalanceAmount = balanceAmount, // Saved explicitly
                    Status = status
                };

                _context.SupplierBills.Add(bill);
                await _context.SaveChangesAsync();

                // 4. Save Products & Increment Stock
                foreach (var item in model.Items)
                {
                    var billProduct = new SupplierBillProduct
                    {
                        SBId = bill.SBId,
                        ProductId = item.ProductId,
                        ProductName = item.ProductName,
                        Price = item.Price,
                        Count = item.Count,
                        TotalPrice = item.TotalPrice
                    };

                    _context.SupplierBillProducts.Add(billProduct);

                    var productInDb = await _context.Products.FindAsync(item.ProductId);
                    if (productInDb != null)
                    {
                        productInDb.StockCount += item.Count;
                       
                        _context.Products.Update(productInDb);
                    }
                }

                // 5. If initial payment exists, record it in payments table
                if (initialPayment > 0)
                {
                    var billPayment = new SupplierBillPayment
                    {
                        SBId = bill.SBId,
                        Payment = initialPayment,
                        Date = model.Date
                    };
                    _context.SupplierBillPayments.Add(billPayment);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Json(new { success = true, redirectUrl = Url.Action(nameof(Index)) });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new { success = false, message = "Error saving bill: " + ex.Message });
            }
        }

        // GET: SupplierBills/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var bill = await _context.SupplierBills
                .Include(b => b.BillProducts)
                .Include(b => b.BillPayments)
                .FirstOrDefaultAsync(m => m.SBId == id);

            if (bill == null) return NotFound();

            return View(bill);
        }

        // GET: SupplierBills/Payment/5
        public async Task<IActionResult> Payment(int? id)
        {
            if (id == null) return NotFound();

            var bill = await _context.SupplierBills.FindAsync(id);
            if (bill == null) return NotFound();

            // Recalculate balance if it was stored as 0
            decimal currentBalance = bill.BalanceAmount;
            if (currentBalance == 0 && bill.TotalAmount > bill.PaidAmount)
            {
                currentBalance = bill.TotalAmount - bill.PaidAmount;
            }

            var viewModel = new MakeSupplierPaymentDto
            {
                SBId = bill.SBId,
                BillNo = bill.BillNo,
                SupplierName = bill.SupplierName,
                TotalAmount = bill.TotalAmount,
                PaidAmount = bill.PaidAmount,
                BalanceAmount = currentBalance,
                Date = DateTime.Now
            };

            return View(viewModel);
        }


        // POST: SupplierBills/Payment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Payment(MakeSupplierPaymentDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var bill = await _context.SupplierBills.FindAsync(model.SBId);
            if (bill == null) return NotFound();

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1. Add record to SupplierBillPayments
                var paymentRecord = new SupplierBillPayment
                {
                    SBId = bill.SBId,
                    Payment = model.Payment,
                    Date = model.Date
                };
                _context.SupplierBillPayments.Add(paymentRecord);

                // 2. Update SupplierBill amounts & status
                bill.PaidAmount += model.Payment;
                bill.BalanceAmount = bill.TotalAmount - bill.PaidAmount;

                if (bill.BalanceAmount <= 0)
                {
                    bill.Status = "Complete";
                }
                else
                {
                    bill.Status = "Balance";
                }

                _context.SupplierBills.Update(bill);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return RedirectToAction(nameof(Details), new { id = bill.SBId });
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