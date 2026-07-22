using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CST8256_BudgetTracker.DataAccess;

public partial class BudgetTrackerContext : DbContext
{
    public BudgetTrackerContext()
    {
    }

    public BudgetTrackerContext(DbContextOptions<BudgetTrackerContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Allocation> Allocations { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Allocation>(entity =>
        {
            entity.HasIndex(e => e.CategoryId, "IX_Allocations_CategoryId");

            entity.Property(e => e.AllocationAmount).HasColumnType("NUMERIC");

            entity.HasOne(d => d.Category).WithMany(p => p.Allocations)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasIndex(e => e.Name, "IX_Categories_Name").IsUnique();
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasIndex(e => e.CategoryId, "IX_Transactions_CategoryId");

            entity.Property(e => e.Amount).HasColumnType("NUMERIC");

            entity.HasOne(d => d.Category).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
