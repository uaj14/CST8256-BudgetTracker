using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CST8256_BudgetTracker.DataAccess;
using CST8256_BudgetTracker.Models;
using NuGet.Protocol;

namespace CST8256_BudgetTracker.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly BudgetTrackerContext _context;

        public TransactionsController(BudgetTrackerContext context)
        {
            _context = context;
        }

        // GET: Transactions
        public async Task<IActionResult> Index(string sort)
        {
            var budgetTrackerContext = _context.Transactions.Include(t => t.Category);

            ViewBag.Date_sort = sort == "Date" ? "Date_desc" : "Date";
            ViewBag.Description_sort = sort == "Description" ? "Description_desc" : "Description";
            ViewBag.Debit_sort = sort == "Debit" ? "Debit_desc" : "Debit";
            ViewBag.Credit_sort = sort == "Credit" ? "Credit_desc" : "Credit";

            // var budgetTrackerContext = _context.Transactions.Include(t => t.Category).AsQueryable();
            var budgetTrackerModel = _context.Transactions
                .Include(t => t.Category)
                .Select(t => new TransactionListItemViewModel
                {
                    // Id = t.Id,
                    Date = t.TransactionDate,
                    Debit = t.TransactionType == "Income" ? t.Amount : null,
                    Credit = t.TransactionType == "Expense" ? t.Amount : null,
                    Description = t.Description
                })
                .AsQueryable();
            switch (sort)
            {
                case "Date":
                    budgetTrackerModel = budgetTrackerModel
                        .OrderBy(t => t.Date);
                    break;
                case "Date_desc":
                    budgetTrackerModel = budgetTrackerModel
                        .OrderByDescending(t => t.Date);
                    break;

                case "Description":
                    budgetTrackerModel = budgetTrackerModel
                        .OrderBy(t => t.Description);
                    break;
                case "Description_desc":
                    budgetTrackerModel = budgetTrackerModel
                        .OrderByDescending(t => t.Description);
                    break;

                case "Debit":
                    budgetTrackerModel = budgetTrackerModel
                        .OrderBy(t => t.Debit==null)
                        .ThenBy(t => t.Debit);
                    break;
                case "Debit_desc":
                    budgetTrackerModel = budgetTrackerModel
                        .OrderByDescending(t => t.Debit);
                    break;

                case "Credit":
                    budgetTrackerModel = budgetTrackerModel
                        .OrderBy(t => t.Credit==null)
                        .ThenBy(t => t.Credit);
                    break;
                case "Credit_desc":
                    budgetTrackerModel = budgetTrackerModel
                        .OrderByDescending(t => t.Credit);
                    break;
            }
            return View(budgetTrackerModel);
            //return View(await budgetTrackerContext.ToListAsync());
        }

        // GET: Transactions/Details/5
        // public async Task<IActionResult> Details(int? id)
        // {
        //     if (id == null)
        //     {
        //         return NotFound();
        //     }

        //     var transaction = await _context.Transactions
        //         .Include(t => t.Category)
        //         .FirstOrDefaultAsync(m => m.Id == id);
        //     if (transaction == null)
        //     {
        //         return NotFound();
        //     }

        //     return View(transaction);
        // }

        // GET: Transactions/Create
        public IActionResult Create()
        {
            //ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id");
            
            // The purpose of the code within the curly braces is to have
            // prepopulated values.
            var model = new TransactionCreateViewModel {
                Date = DateOnly.FromDateTime(DateTime.Today), // AI: Found way to prepopulate field with current date.
                TransactionTypeOptions = GetTransactionTypesOptions(),
                CategoryOptions = GetCategoriesOptions()
            };
            
            return View(model);
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Create([Bind("Id,Amount,TransactionDate,TransactionType,CreatedAt,UpdatedAt,CategoryId,Description")] Transaction transaction)
        public async Task<IActionResult> Create(TransactionCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Process the form. Convert from the VM to Transaction proper.
                var transaction = new Transaction
                {
                    // Id = 1,
                    Amount = (double)model.Amount,
                    TransactionDate = model.Date,
                    TransactionType = model.TransactionType,
                    Description = model.Description,
                    CategoryId = model.CategoryId,
                    CreatedAt = DateTime.UtcNow.ToString(),
                    UpdatedAt = DateTime.UtcNow.ToString()
                };

                _context.Add(transaction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Invalid data
            // Repopulate the DDLs
            model.TransactionTypeOptions = GetTransactionTypesOptions();
            model.CategoryOptions = GetCategoriesOptions();
            return View(model);

            // ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", transaction.CategoryId);
            // return View(transaction);
        }

        // GET: Transactions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", transaction.CategoryId);
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Amount,TransactionDate,TransactionType,CreatedAt,UpdatedAt,CategoryId,Description")] Transaction transaction)
        {
            if (id != transaction.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transaction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransactionExists(transaction.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", transaction.CategoryId);
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        // public async Task<IActionResult> Delete(int? id)
        // {
        //     if (id == null)
        //     {
        //         return NotFound();
        //     }

        //     var transaction = await _context.Transactions
        //         .Include(t => t.Category)
        //         .FirstOrDefaultAsync(m => m.Id == id);
        //     if (transaction == null)
        //     {
        //         return NotFound();
        //     }

        //     return View(transaction);
        // }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransactionExists(int id)
        {
            return _context.Transactions.Any(e => e.Id == id);
        }

        // Create select lists for transactions.
        private List<SelectListItem> GetTransactionTypesOptions() {
            return new List<SelectListItem> { // Asked AI on how to quickly hardcode a new SelectListItem.
                    new SelectListItem { Text = "Expense", Value = "Expense" },
                    new SelectListItem { Text = "Income", Value = "Income" }
                };
        }
        private List<SelectListItem> GetCategoriesOptions() {
            return _context.Categories
                    .Select(c => new SelectListItem {
                        Value = c.Id.ToString(),
                        Text = c.Name
                    }).ToList();
        }

    }
}
