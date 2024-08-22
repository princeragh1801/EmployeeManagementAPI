using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeSystem.Contract.Models
{
    public class BaseEntity
    {
        public int ?CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        public Employee ?Creator { get; set; }
        [ForeignKey(nameof(UpdatedBy))]
        public Employee ?Updator { get; set; }
    }
}
