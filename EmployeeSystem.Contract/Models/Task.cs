using System.ComponentModel.DataAnnotations.Schema;
using static EmployeeSystem.Contract.Enums.Enums;

namespace EmployeeSystem.Contract.Models
{
    public class Tasks : BaseEntity
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public required string Description { get; set; }

        public int? AssignedTo { get; set; }

        public int AssignedBy { get; set; }

        public TasksStatus Status { get; set; } = TasksStatus.Pending;

        public int? ProjectId { get; set; } = null;

        [ForeignKey(nameof(AssignedTo))]
        public Employee Employee { get; set; }

        [ForeignKey(nameof(AssignedBy))]
        public Employee Admin { get; set; }

        [ForeignKey(nameof(ProjectId))]
        public Project ?Project { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
