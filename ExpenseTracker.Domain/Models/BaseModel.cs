using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Domain.Models {
    public class BaseModel {

        [Column(TypeName = "smalldatetime")]
        public DateTime? DateCreated { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? DateModified { get; set; }
    }
}
