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

        public TasksStatus Status { get; set; } = TasksStatus.Pending;

        public TaskType TaskType { get; set; } = TaskType.Epic;

        public int? ParentId { get; set; }

        public int ProjectId { get; set; }

        public int? SprintId { get; set; }

        public int? OriginalEstimateHours { get; set; }

        public int? RemainingEstimateHours { get; set; }

        [ForeignKey(nameof(SprintId))]
        public Sprint ?Sprint { get; set; }

        [ForeignKey(nameof(ParentId))]
        public Tasks? Parent { get; set; }

        [ForeignKey(nameof(AssignedTo))]
        public Employee? Employee { get; set; }

        [ForeignKey(nameof(ProjectId))]
        public Project Project { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
