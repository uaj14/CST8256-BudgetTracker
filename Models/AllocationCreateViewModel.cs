using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CST8256_BudgetTracker.Models;

public class AllocationCreateViewModel
{
    public List<SelectListItem> CategoryOptions { get; set; } = new();

    public int CategoryId { get; set; }

    [Required]
    [Range(0, Int32.MaxValue)]
    public double? Amount { get; set; }
}