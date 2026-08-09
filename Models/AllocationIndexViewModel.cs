using Microsoft.AspNetCore.Mvc.Rendering;
using CST8256_BudgetTracker.DataAccess;

namespace CST8256_BudgetTracker.Models;
public class AllocationIndexViewModel
{
    public List<Allocation> Allocations { get; set; } = new();
    public List<SelectListItem> AllocationMonthOptions { get; set; } = new();
    public string? SelectedAllocationMonth { get; set; }
}
