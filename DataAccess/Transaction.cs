using System;
using System.Collections.Generic;

namespace CST8256_BudgetTracker.DataAccess;

public partial class Transaction
{
    public int Id { get; set; }

    public double Amount { get; set; }

    public DateOnly TransactionDate { get; set; }

    public string TransactionType { get; set; } = null!;

    public string CreatedAt { get; set; } = null!;

    public string UpdatedAt { get; set; } = null!;

    public int CategoryId { get; set; }

    public string? Description { get; set; }

    public virtual Category Category { get; set; } = null!;
}
