using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Domain.Models {
    public class Category {

        [Key]
        public int CategoryID { get; set; }

        [Required]
        [StringLength(60)]
        [Display(Name = "Category name")]
        public string CategoryName { get; set; }
    }
}
