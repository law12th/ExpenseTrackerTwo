using ExpenseTracker.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Infrastructure {
    public class ExpenseTrackerContext: DbContext {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Expense> Expenses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            string connectionString = "Data Source=INTERN1\\SMARTCARE40;Initial Catalog=ExpenseTracker;User ID=sa;Password=N0WhereMan";
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
