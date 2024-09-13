using System.ComponentModel.DataAnnotations;
using EmployeeSystem.Contract.Enums;

namespace EmployeeSystem.Contract.Dtos.Add
{
    public class UpdateTaskDto
    {
        public string? Name { get; set; }

        public string? Description { get; set; }

        public int? AssignedTo { get; set; }

        public TaskType? TaskType { get; set; }

        public int? ParentId { get; set; }

        public int? ProjectId { get; set; }

        public int? OriginalEstimateHours { get; set; }

        public int? RemainingEstimateHours { get; set; }

        public int? SprintId { get; set; } = null;
        [Required]
        public TasksStatus? Status { get; set; } = TasksStatus.Pending;
    }
}
