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
    public class AllocationsController : Controller
    {
        private readonly BudgetTrackerContext _context;

        public AllocationsController(BudgetTrackerContext context)
        {
            _context = context;
        }

        // GET: Allocations
        // Prompted ChatGPT for MVC implementation of toggle-sort functionality.
        // It recommended using ViewBag and `.AsQueryable()`.
        public async Task<IActionResult> Index(string sort)
        {
            ViewBag.AllocationAmount_sort = sort == "AllocationAmount" ? "AllocationAmount_desc" : "AllocationAmount";
            ViewBag.AllocationMonth_sort = sort == "AllocationMonth" ? "AllocationMonth_desc" : "AllocationMonth";
            ViewBag.CreatedAt_sort = sort == "CreatedAt" ? "CreatedAt_desc" : "CreatedAt";
            ViewBag.UpdatedAt_sort = sort == "UpdatedAt" ? "UpdatedAt_desc" : "UpdatedAt";

            var budgetTrackerContext = _context.Allocations.Include(a => a.Category).AsQueryable();
            switch (sort)
            {
                case "AllocationAmount":
                    budgetTrackerContext = budgetTrackerContext
                        .OrderBy(a => a.AllocationAmount);
                    break;
                case "AllocationAmount_desc":
                    budgetTrackerContext = budgetTrackerContext
                        .OrderByDescending(a => a.AllocationAmount);
                    break;

                case "AllocationMonth":
                    budgetTrackerContext = budgetTrackerContext
                        .OrderBy(a => a.AllocationMonth);
                    break;
                case "AllocationMonth_desc":
                    budgetTrackerContext = budgetTrackerContext
                        .OrderByDescending(a => a.AllocationMonth);
                    break;

                case "CreatedAt":
                    budgetTrackerContext = budgetTrackerContext
                        .OrderBy(a => a.CreatedAt);
                    break;
                case "CreatedAt_desc":
                    budgetTrackerContext = budgetTrackerContext
                        .OrderByDescending(a => a.CreatedAt);
                    break;

                case "UpdatedAt":
                    budgetTrackerContext = budgetTrackerContext
                        .OrderBy(a => a.UpdatedAt);
                    break;
                case "UpdatedAt_desc":
                    budgetTrackerContext = budgetTrackerContext
                        .OrderByDescending(a => a.UpdatedAt);
                    break;
            }

            return View(await budgetTrackerContext.ToListAsync());
        }

        // GET: Allocations/Details/5
        // public async Task<IActionResult> Details(int? id)
        // {
        //     if (id == null)
        //     {
        //         return NotFound();
        //     }

        //     var allocation = await _context.Allocations
        //         .Include(a => a.Category)
        //         .FirstOrDefaultAsync(m => m.Id == id);
        //     if (allocation == null)
        //     {
        //         return NotFound();
        //     }

        //     return View(allocation);
        // }

        // GET: Allocations/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id");
            return View();
        }

        // POST: Allocations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AllocationAmount,AllocationMonth,CreatedAt,UpdatedAt,CategoryId")] Allocation allocation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(allocation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", allocation.CategoryId);
            return View(allocation);
        }

        // GET: Allocations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var allocation = await _context.Allocations.FindAsync(id);
            if (allocation == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", allocation.CategoryId);
            return View(allocation);
        }

        // POST: Allocations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AllocationAmount,AllocationMonth,CreatedAt,UpdatedAt,CategoryId")] Allocation allocation)
        {
            if (id != allocation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(allocation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AllocationExists(allocation.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", allocation.CategoryId);
            return View(allocation);
        }

        // GET: Allocations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var allocation = await _context.Allocations
                .Include(a => a.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (allocation == null)
            {
                return NotFound();
            }

            return View(allocation);
        }

        // POST: Allocations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAllocation(int id)
        {
            var allocation = await _context.Allocations.FindAsync(id);
            if (allocation != null)
            {
                _context.Allocations.Remove(allocation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AllocationExists(int id)
        {
            return _context.Allocations.Any(e => e.Id == id);
        }
    }
}
