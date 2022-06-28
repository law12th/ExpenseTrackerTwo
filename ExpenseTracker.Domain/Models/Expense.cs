using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Domain.Models {
    public class Expense {

        [Key]
        public int ExpenseID { get; set; }

        [Required]
        public DateTime ExpenseDate { get; set; }

        [Required]
        public Decimal Amount { get; set; }

        public int CategoryID { get; set; }

        [ForeignKey("CategoryID")]
        public virtual Category Category { get; set; }
    }
}
