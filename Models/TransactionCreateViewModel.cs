using Microsoft.AspNetCore.Mvc.Rendering;

namespace CST8256_BudgetTracker.Models;

public class TransactionCreateViewModel
{
    public int Id { get; set; }
    public double? Amount { get; set; }

    public DateOnly Date { get; set; }

    public string TransactionType { get; set; } = null!;

    public int CategoryId { get; set; }

    public string? Description { get; set; } = string.Empty;

    public List<SelectListItem> TransactionTypeOptions { get; set; } = new();

    public List<SelectListItem> CategoryOptions { get; set; } = new();

    // public virtual Category Category { get; set; } = null!;
}
