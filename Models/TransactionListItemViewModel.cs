// namespace CST8256_BudgetTracker;
namespace CST8256_BudgetTracker.Models;
public class TransactionListItemViewModel
{
    public int Id { get; set; }
    public double Debit { get; set; }

    public double Credit { get; set; }

    public DateOnly Date { get; set; }

    public string TransactionType { get; set; } = null!;

    // public int CategoryId { get; set; }

    public string? Description { get; set; }

    // public virtual Category Category { get; set; } = null!;
}
