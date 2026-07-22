using System;
using System.Collections.Generic;

namespace CST8256_BudgetTracker.DataAccess;

public partial class Allocation
{
    public int Id { get; set; }

    public double AllocationAmount { get; set; }

    public DateOnly AllocationMonth { get; set; }

    public string CreatedAt { get; set; } = null!;

    public string UpdatedAt { get; set; } = null!;

    public int CategoryId { get; set; }

    public virtual Category Category { get; set; } = null!;
}
