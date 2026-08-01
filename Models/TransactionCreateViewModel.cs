using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CST8256_BudgetTracker.Models;

public class TransactionCreateViewModel
{
    public int Id { get; set; }
    public double? Amount { get; set; }

    public DateOnly Date { get; set; }
    
    [Display(Name = "Type")]
    public string TransactionType { get; set; } = null!;

    [Display(Name = "Category")]    // AI: Changing the text displayed in label.
    public int CategoryId { get; set; }

    public string? Description { get; set; } = string.Empty;

    public List<SelectListItem> TransactionTypeOptions { get; set; } = new();

    public List<SelectListItem> CategoryOptions { get; set; } = new();

    // public virtual Category Category { get; set; } = null!;
}
