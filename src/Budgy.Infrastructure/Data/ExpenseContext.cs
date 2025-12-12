using Microsoft.EntityFrameworkCore;
using Budgy.Domain.Entities;

/// <summary>
/// Represents the Entity Framework Core database context for the Expense Tracker application.
/// Manages the connection to the SQLite database and provides access to the Users and Expenses tables.
/// </summary>
public class ExpenseContext : DbContext
{

    public ExpenseContext(DbContextOptions<ExpenseContext> options) : base(options) { }

    /// <summary>
    /// Gets or sets the Users table in the database.
    /// </summary>
    public DbSet<User> Users { get; set; }

    /// <summary>
    /// Gets or sets the Expenses table in the database.
    /// </summary>
    public DbSet<Expense> Expenses { get; set; }

    /// <summary>
    /// Configures the database connection to use a local SQLite database file named "expenses.db".
    /// </summary>
    /// <param name="optionsBuilder">The builder used to configure the database context options.</param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=Budgy.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<Expense>().ToTable("Expenses");
    }

}