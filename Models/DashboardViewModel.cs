using Microsoft.AspNetCore.Mvc.Rendering;
using CST8256_BudgetTracker.DataAccess;

namespace CST8256_BudgetTracker.Models;

public class DashboardViewModel
{
    public List<Allocation> Allocations { get; set; } = new();
    public List<Transaction> Transactions { get; set; } = new();
    public AllocationIndexViewModel AllocationVM { get; set; } = new();

    public List<SelectListItem> MonthOptions { get; set; } = new();
    public string? SelectedMonth { get; set; }
}