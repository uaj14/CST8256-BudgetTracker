using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CST8256_BudgetTracker.DataAccess;

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
            // var budgetTrackerContext = _context.Transactions.Include(t => t.Category);

            ViewBag.TransactionDate_sort = sort == "TransactionDate" ? "TransactionDate_desc" : "TransactionDate";
            ViewBag.Description_sort = sort == "Description" ? "Description_desc" : "Description";
            ViewBag.Amount_sort = sort == "Amount" ? "Amount_desc" : "Amount";

            var budgetTrackerContext = _context.Transactions.Include(t => t.Category).AsQueryable();
            switch (sort)
            {
                case "TransactionDate":
                    budgetTrackerContext = budgetTrackerContext
                        .OrderBy(t => t.TransactionDate);
                    break;
                case "TransactionDate_desc":
                    budgetTrackerContext = budgetTrackerContext
                        .OrderByDescending(t => t.TransactionDate);
                    break;

                case "Description":
                    budgetTrackerContext = budgetTrackerContext
                        .OrderBy(t => t.Description);
                    break;
                case "Description_desc":
                    budgetTrackerContext = budgetTrackerContext
                        .OrderByDescending(t => t.Description);
                    break;

                case "Amount":
                    budgetTrackerContext = budgetTrackerContext
                        .OrderBy(t => t.Amount);
                    break;
                case "Amount_desc":
                    budgetTrackerContext = budgetTrackerContext
                        .OrderByDescending(t => t.Amount);
                    break;
            }
            return View(await budgetTrackerContext.ToListAsync());
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id");
            return View();
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Amount,TransactionDate,TransactionType,CreatedAt,UpdatedAt,CategoryId,Description")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                _context.Add(transaction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", transaction.CategoryId);
            return View(transaction);
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
    }
}
