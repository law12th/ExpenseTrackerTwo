using ExpenseTracker.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Infrastructure {
    public class ExpenseTrackerContext: DbContext {
        public DbSet<Category> categories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            string connectionString = "Database=INTERN1;Initial Catalog=ExpenseTracker;User id=sa";
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
