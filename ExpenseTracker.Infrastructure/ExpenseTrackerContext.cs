using ExpenseTracker.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Infrastructure {
    public class ExpenseTrackerContext: DbContext {
        public DbSet<Category> categories { get; set; }
        public DbSet<Expense> expenses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            string connectionString = "Data Source=INTERN1\\SMARTCARE40;Initial Catalog=ExpenseTracker;User ID=sa;Password=N0WhereMan";
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
