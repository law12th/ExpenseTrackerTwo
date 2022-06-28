using ExpenseTracker.Utilities.Constants;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Domain.Models {
    public class Category {

        [Key]
        public int CategoryID { get; set; }

        [Required(ErrorMessage = MessageConstants.RequiredError)]
        [StringLength(60)]
        [Display(Name = "Category name")]
        public string CategoryName { get; set; }
    }
}
