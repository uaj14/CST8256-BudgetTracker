using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CST8256_BudgetTracker.DataAccess;

public partial class Transaction
{
    public int Id { get; set; }

    public double Amount { get; set; }

    [Display(Name = "Date")]    // Used ChatGPT to find a way to change header name without resorting to static text.
                                // Note: This change may have been unnecessary in the end.
    public DateOnly TransactionDate { get; set; }

    public string TransactionType { get; set; } = null!;

    public string CreatedAt { get; set; } = null!;

    public string UpdatedAt { get; set; } = null!;

    public int CategoryId { get; set; }

    public string? Description { get; set; }

    public virtual Category Category { get; set; } = null!;
}
