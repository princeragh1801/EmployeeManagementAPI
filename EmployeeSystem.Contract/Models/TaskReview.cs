using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeSystem.Contract.Models
{
    public class TaskReview : BaseEntity
    {
        public int Id { get; set; }

        public int TaskID { get; set; }

        public string Content { get; set; }

        [ForeignKey(nameof(TaskID))]
        public Tasks Task { get; set; }

        public bool IsActive { get; set; } = true;

    }
}
