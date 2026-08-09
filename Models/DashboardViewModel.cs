using Microsoft.AspNetCore.Mvc.Rendering;
using CST8256_BudgetTracker.DataAccess;
using System.ComponentModel.DataAnnotations;

namespace CST8256_BudgetTracker.Models;

public class DashboardViewModel
{
    public List<Allocation> Allocations { get; set; } = new();
    public List<Transaction> Transactions { get; set; } = new();
    public AllocationIndexViewModel AllocationVM { get; set; } = new();
    public List<DashboardRemainingAllocationsViewModel> RemainingAllocationsVM { get; set; } = new();
    public List<SelectListItem> MonthOptions { get; set; } = new();
    public string? SelectedMonth { get; set; }
}

public class DashboardRemainingAllocationsViewModel
{
    public string CategoryName { get; set; }
    [DisplayFormat(DataFormatString = "{0:C}")] // AI mentioned this as an alternative to .ToString("C") in the View.
    public double Allocated { get; set; }
    [DisplayFormat(DataFormatString = "{0:C}")]
    public double Spent { get; set; }
    [DisplayFormat(DataFormatString = "{0:C}")]
    public double Remaining { get; set; }
}