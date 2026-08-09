using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CST8256_BudgetTracker.DataAccess;
using CST8256_BudgetTracker.Models;
using Microsoft.IdentityModel.Tokens;

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
        public async Task<IActionResult> Index(string sort, string? SelectedAllocationMonth)
        {
            var allocationsContext = _context.Allocations.Include(a => a.Category).AsQueryable();

            // Prompted ChatGPT to create DDL items for available months from the DB
            var allocationsModel = new AllocationIndexViewModel();

            allocationsModel.AllocationMonthOptions = _context.Allocations
                .Select(a => new
                {
                    Year = a.AllocationMonth.Year,
                    Month = a.AllocationMonth.Month
                })
                .Distinct()
                .OrderByDescending(x => x.Year)
                .ThenByDescending(x => x.Month)
                .AsEnumerable()
                .Select(x => new SelectListItem
                {
                    Value = $"{x.Year}-{x.Month:D2}",
                    Text = new DateTime(x.Year, x.Month, 1)
                        .ToString("MMMM yyyy")
                })
                .ToList();

            // Default option
            allocationsModel.AllocationMonthOptions.Insert(0, new SelectListItem
                {
                    Value = "",
                    Text = "-- All Months --"
                });

            // Parse selected month and year.
            if (!string.IsNullOrEmpty(SelectedAllocationMonth))
            {
                var parts = SelectedAllocationMonth.Split('-');

                int searchYear = int.Parse(parts[0]);
                int searchMonth = int.Parse(parts[1]);

                // Select only allocations from the selected month.
                allocationsContext = allocationsContext
                    .Where(a =>
                        a.AllocationMonth.Year == searchYear &&
                        a.AllocationMonth.Month == searchMonth);
            }

            ViewBag.AllocationAmount_sort = sort == "AllocationAmount" ? "AllocationAmount_desc" : "AllocationAmount";
            ViewBag.AllocationMonth_sort = sort == "AllocationMonth" ? "AllocationMonth_desc" : "AllocationMonth";
            ViewBag.CreatedAt_sort = sort == "CreatedAt" ? "CreatedAt_desc" : "CreatedAt";
            ViewBag.UpdatedAt_sort = sort == "UpdatedAt" ? "UpdatedAt_desc" : "UpdatedAt";

            // Sort
            switch (sort)
            {
                case "AllocationAmount":
                    allocationsContext = allocationsContext
                        .OrderBy(a => a.AllocationAmount);
                    break;
                case "AllocationAmount_desc":
                    allocationsContext = allocationsContext
                        .OrderByDescending(a => a.AllocationAmount);
                    break;

                case "AllocationMonth":
                    allocationsContext = allocationsContext
                        .OrderBy(a => a.AllocationMonth);
                    break;
                case "AllocationMonth_desc":
                    allocationsContext = allocationsContext
                        .OrderByDescending(a => a.AllocationMonth);
                    break;

                case "CreatedAt":
                    allocationsContext = allocationsContext
                        .OrderBy(a => a.CreatedAt);
                    break;
                case "CreatedAt_desc":
                    allocationsContext = allocationsContext
                        .OrderByDescending(a => a.CreatedAt);
                    break;

                case "UpdatedAt":
                    allocationsContext = allocationsContext
                        .OrderBy(a => a.UpdatedAt);
                    break;
                case "UpdatedAt_desc":
                    allocationsContext = allocationsContext
                        .OrderByDescending(a => a.UpdatedAt);
                    break;
            }

            allocationsModel.Allocations = await allocationsContext.ToListAsync();
            return View(allocationsModel);
        }

        // GET: Allocations/Create
        public IActionResult Create()
        {
            var model = new AllocationCreateViewModel {
                CategoryOptions = GetCategoriesOptions()
            };
            
            return View(model);
        }

        // POST: Allocations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AllocationCreateViewModel model)
        {
            ViewBag.ErrorMessage = "";

            // Create new allocation iff there is no existing allocation for the current month.
            DateOnly currentMonth = new DateOnly(DateTime.Today.Year, DateTime.Today.Month, 1);
            var potential_record = await _context.Allocations
                .FirstOrDefaultAsync(a => a.CategoryId == model.CategoryId && a.AllocationMonth == currentMonth);

            if (potential_record == null) {
                if (ModelState.IsValid)
                {
                    // Process the form. Convert from the VM to Allocation proper.
                    var allocation = new Allocation
                    {
                        // Id = 1,
                        AllocationAmount = (double)model.Amount,
                        AllocationMonth = currentMonth,
                        CategoryId = model.CategoryId,
                        CreatedAt = DateTime.UtcNow.ToString(),
                        UpdatedAt = DateTime.UtcNow.ToString()
                    };

                    _context.Allocations.Add(allocation);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            // Invalid data
            ViewBag.ErrorMessage = "This allocation already exists for this month.";
            // Repopulate the DDLs
            model.CategoryOptions = GetCategoriesOptions();
            return View(model);
        }

        // GET: Allocations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.MonthErrorMessage = "";
            ViewBag.IsCurrentMonth = true;

            if (id == null)
            {
                return NotFound();
            }

            var allocation = await _context.Allocations.FindAsync(id);
            if (allocation == null)
            {
                return NotFound();
            }

            // The purpose of the code within the curly braces is to have prepopulated values.
            var model = new AllocationCreateViewModel {
                Id = allocation.Id,
                Amount = allocation.AllocationAmount,
                CategoryId = allocation.CategoryId,
                CategoryOptions = GetCategoriesOptions()
            };

            // Only show Edit form if allocation is from current month.
            if (!(allocation.AllocationMonth == new DateOnly(DateTime.Today.Year, DateTime.Today.Month, 1)))
            {
                ViewBag.IsCurrentMonth = false;
                ViewBag.MonthErrorMessage = "Cannot edit allocations from previous months.";
            }

            return View(model);
        }

        // POST: Allocations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AllocationCreateViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            // Used AI to process the update for an existing transaction
            // Load entity from database
            var allocation = await _context.Allocations.FindAsync(id);

            if (ModelState.IsValid)
            {
                try
                {
                // Can only edit allocations for the current month
                if (allocation.AllocationMonth == new DateOnly(DateTime.Today.Year, DateTime.Today.Month, 1))
                {
                                        // AI: Copy values from the view model
                    allocation.AllocationAmount = (double)model.Amount;
                    allocation.CategoryId = model.CategoryId;
                    // _context.Update(allocation);
                    await _context.SaveChangesAsync();
                }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AllocationExists(model.Id))
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
            model.CategoryOptions = GetCategoriesOptions();
            return View(model);
            // ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", allocation.CategoryId);
            // return View(allocation);
        }

        // GET: Allocations/Delete/5
        // public async Task<IActionResult> Delete(int? id)
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

        // POST: Allocations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAllocation(int id)
        {
            var allocation = await _context.Allocations.FindAsync(id);
            if (allocation != null)
            {

                // Only delete if allocation is from current month.
                if (allocation.AllocationMonth == new DateOnly(DateTime.Today.Year, DateTime.Today.Month, 1))
                {
                    _context.Allocations.Remove(allocation);
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AllocationExists(int id)
        {
            return _context.Allocations.Any(e => e.Id == id);
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
