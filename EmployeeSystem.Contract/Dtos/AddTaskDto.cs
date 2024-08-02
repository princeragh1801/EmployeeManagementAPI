using System.ComponentModel.DataAnnotations;
using static EmployeeSystem.Contract.Enums.Enums;

namespace EmployeeSystem.Contract.Dtos
{
    public class AddTaskDto
    {
        [Required(ErrorMessage = "Name is required")]
        [MinLength(2)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [MinLength(2)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Id of Assignee is required")]
        [Range(1, Int32.MaxValue)]
        public int AssignedTo { get; set; }

        [Required(ErrorMessage = "Id of Assigner is required")]
        [Range(1, Int32.MaxValue)]
        public int AssignedBy { get; set; }

        public int? ProjectId { get; set; } = null;

        [Required]
        public TasksStatus Status { get; set; } = TasksStatus.Pending;
    }
}
