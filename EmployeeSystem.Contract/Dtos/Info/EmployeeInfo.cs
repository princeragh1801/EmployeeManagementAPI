using static EmployeeSystem.Contract.Enums.Enums;

namespace EmployeeSystem.Contract.Dtos.Info
{
    public class EmployeeInfo
    {
        public int? Id { get; set; }

        public string Name { get; set; }

        public string? DepartmentName { get; set; }

        public string? ManagerName { get; set; }

        public Role Role { get; set; } = Role.Employee;

        public decimal Salary { get; set; }
        
        public string Email { get; set; }

        public string Address { get; set; }

        public string ImageUrl { get; set; }

        public string Phone { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
