using System;
using System.Collections.Generic;

namespace CST8256_BudgetTracker.DataAccess;

public partial class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Allocation> Allocations { get; set; } = new List<Allocation>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
