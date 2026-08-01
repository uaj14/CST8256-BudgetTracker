using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CST8256_BudgetTracker.Models;

public class TransactionCreateViewModel
{
    public int Id { get; set; }
    [Required]
    [Range(0, Int32.MaxValue)]  // https://stackoverflow.com/questions/7256753/min-max-value-validators-in-asp-net-mvc
    public double? Amount { get; set; }

    public DateOnly Date { get; set; }

    [Required]
    [AllowedValues("Expense", "Income", ErrorMessage = "Invalid transaction type.")] // AI: Direct string compare.
    [Display(Name = "Type")]
    public string TransactionType { get; set; } = null!;

    [Required]
    [Display(Name = "Category")]    // AI: Changing the text displayed in label.
    [Range(1, 10)]
    public int CategoryId { get; set; }

    public string? Description { get; set; } = string.Empty;

    public List<SelectListItem> TransactionTypeOptions { get; set; } = new();

    public List<SelectListItem> CategoryOptions { get; set; } = new();

    // public virtual Category Category { get; set; } = null!;
}
