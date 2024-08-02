using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EmployeeSystem.Contract.Enums.Enums;
namespace EmployeeSystem.Contract.Dtos
{
    public class EmployeeDto : BaseEntityDto
    {
        public int ?Id { get; set; }

        public string Name { get; set; }
        public string? DepartmentName { get; set; }
        public string? ManagerName { get; set; }
        public  Role Role { get; set; } = Role.Employee;

        public decimal Salary { get; set; }
        public int? DepartmentId { get; set; }
        public int? ManagerId { get; set; }
    }
}
