using System.ComponentModel.DataAnnotations;
using EmployeeSystem.Contract.Enums;

namespace EmployeeSystem.Contract.Dtos.Add
{
    public class AddTaskDto
    {
        [Required(ErrorMessage = "Name is required")]
        [MinLength(2, ErrorMessage = "Name must be at least 2 characters long")]
        [MaxLength(50, ErrorMessage = "Name must not exceed 50 characters")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [MinLength(10, ErrorMessage = "Name must be at least 10 characters long")]
        public required string Description { get; set; }

        public int ?AssignedTo { get; set; }

        public TaskType TaskType { get; set; }

        public int ?ParentId { get; set; }
        
        public int? OriginalEstimateHours { get; set; }

        public int ProjectId { get; set; }
        public int? SprintId { get; set; } = null;
        [Required]
        public TasksStatus Status { get; set; } = TasksStatus.Pending;
    }
}
