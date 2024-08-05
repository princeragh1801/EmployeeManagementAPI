using EmployeeSystem.Contract.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EmployeeSystem.Contract.Enums.Enums;

namespace EmployeeSystem.Contract.Dtos
{
    public class UpdateEmployeeDto
    {
        [Required(ErrorMessage = "Name is required")]
        [MinLength(2)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Salary is required")]
        [Range(1, Double.MaxValue)]
        public decimal Salary { get; set; }

        public int? DepartmentID { get; set; }
        public int? ManagerID { get; set; }

        public Role Role { get; set; } = Role.Employee;
    }
}
