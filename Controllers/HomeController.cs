using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CST8256_BudgetTracker.Models;
using CST8256_BudgetTracker.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CST8256_BudgetTracker.Controllers;

public class HomeController : Controller
{
    private readonly BudgetTrackerContext _context;

    public HomeController(BudgetTrackerContext context)
    {
        _context = context;
    }
    public async Task<IActionResult> Index(string? SelectedMonth)
    {
        // Month selected
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

        // Parse selected month and year.
        // Default values for search year and month.
        int searchYear = @DateTime.Today.Year;
        int searchMonth = @DateTime.Today.Month;

        if (!string.IsNullOrEmpty(SelectedMonth))
        {
            var parts = SelectedMonth.Split('-');

            searchYear = int.Parse(parts[0]);
            searchMonth = int.Parse(parts[1]);

            allocationsContext = allocationsContext
                .Where(a =>
                    a.AllocationMonth.Year == searchYear &&
                    a.AllocationMonth.Month == searchMonth);
        }

        // For displaying currently selected month.
        ViewBag.searchYear = searchYear;
        ViewBag.searchMonth = searchMonth;

        // Total income this month
        ViewBag.SelectedMonthIncome = await _context.Transactions
            .Include(t => t.Category)
            .Where(t =>
                t.TransactionDate.Year == searchYear &&
                t.TransactionDate.Month == searchMonth &&
                t.Category.Name == "Income")
            .SumAsync(t => t.Amount);

        // Total expenses this month
        ViewBag.SelectedMonthExpense = await _context.Transactions
            .Include(t => t.Category)
            .Where(t =>
                t.TransactionDate.Year == searchYear &&
                t.TransactionDate.Month == searchMonth &&
                t.Category.Name != "Income")
            .SumAsync(t => t.Amount);

        // Recent 5 transactions of a given month
        var model = new DashboardViewModel
        {
            Allocations = _context.Allocations
                .Include(a => a.Category)
                .Where(a =>
                    a.AllocationMonth.Year == searchYear &&
                    a.AllocationMonth.Month == searchMonth)
                .ToList(),
            Transactions = _context.Transactions
                .Where(t =>
                    t.TransactionDate.Year == searchYear &&
                    t.TransactionDate.Month == searchMonth)
                .OrderByDescending(t => t.TransactionDate)
                .Take(5)
                .ToList(),
            AllocationVM = allocationsModel
        };

        // Remaining allocations

        return View(model);
    }

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
