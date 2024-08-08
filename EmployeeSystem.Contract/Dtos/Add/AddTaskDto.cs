using System.ComponentModel.DataAnnotations;
using static EmployeeSystem.Contract.Enums.Enums;

namespace EmployeeSystem.Contract.Dtos.Add
{
    public class AddTaskDto
    {
        [Required(ErrorMessage = "Name is required")]
        [MinLength(2, ErrorMessage = "Name must be at least 2 characters long")]
        [MaxLength(50, ErrorMessage = "Name must not exceed 50 characters")]
        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Name can only contain lowercase and uppercase letters and spaces")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [MinLength(10, ErrorMessage = "Name must be at least 10 characters long")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Id of Assignee is required")]
        [Range(1, int.MaxValue)]
        public int AssignedTo { get; set; }

        /*[Required(ErrorMessage = "Id of Assigner is required")]
        [Range(1, Int32.MaxValue)]
        public int AssignedBy { get; set; }*/

        public int? ProjectId { get; set; } = null;

        [Required]
        public TasksStatus Status { get; set; } = TasksStatus.Pending;
    }
}
