using System.ComponentModel.DataAnnotations;

namespace EmployeeSystem.Contract.Dtos
{
    public class AttendanceDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Employee id is required")]
        [Range(1, int.MaxValue)]
        public int employeeId { get; set; }

        public DateOnly DateOnly { get; set; } = new DateOnly();

        public EmployeeDto Employee { get; set; }
    }
}
