using System.ComponentModel.DataAnnotations;
using static EmployeeSystem.Contract.Enums.Enums;

namespace EmployeeSystem.Contract.Dtos.Add
{
    public class UpdateEmployeeDto
    {
        
        [Required(ErrorMessage = "Name is required")]
        [MinLength(2, ErrorMessage = "Name must be at least 2 characters long")]
        [MaxLength(50, ErrorMessage = "Name must not exceed 50 characters")]
        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Name can only contain lowercase and uppercase letters and spaces")]
        public string Name { get; set; }



        [Required(ErrorMessage = "Email is required")]
        [MaxLength(100, ErrorMessage = "Email must not exceed 100 characters")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email address format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [MaxLength(200, ErrorMessage = "Address must not exceed 200 characters")]
        public string Address { get; set; }

        [RegularExpression("^[0-9]{10}$", ErrorMessage = "Phone number must be exactly 10 digits long")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Salary is required")]
        [Range(1, double.MaxValue)]
        public decimal Salary { get; set; }

        public int? DepartmentID { get; set; }
        public int? ManagerID { get; set; }

        public Role Role { get; set; } = Role.Employee;
    }
}
