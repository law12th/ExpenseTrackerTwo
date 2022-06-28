using ExpenseTracker.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Infrastructure {
    public class ExpenseTrackerContext: DbContext {
        public DbSet<Category> categories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            string connectionString = "server=(localdb)\\mssqldb; database=ExpenseTracker";
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
