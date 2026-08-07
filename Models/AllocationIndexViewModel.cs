using Microsoft.AspNetCore.Mvc.Rendering;
using CST8256_BudgetTracker.DataAccess;

namespace CST8256_BudgetTracker.Models;
public class AllocationIndexViewModel
{
    // public int Id { get; set; }

    // public double AllocationAmount { get; set; }

    // public DateOnly AllocationMonth { get; set; }

    // public int CategoryId { get; set; }

    public List<Allocation> Allocations { get; set; } = new();
    public List<SelectListItem> AllocationMonthOptions { get; set; } = new();

    public string? SelectedAllocationMonth { get; set; }

    // public virtual Category Category { get; set; } = null!;

}
