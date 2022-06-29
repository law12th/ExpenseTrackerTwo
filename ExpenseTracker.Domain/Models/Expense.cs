using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Domain.Models {
    public class Expense: BaseModel {

        [Key]
        public int ExpenseID { get; set; }

        [Required]
        public DateTime ExpenseDate { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 6)")]
        public Decimal Amount { get; set; }

        public int CategoryID { get; set; }

        [ForeignKey("CategoryID")]
        public virtual Category Category { get; set; }
    }
}
