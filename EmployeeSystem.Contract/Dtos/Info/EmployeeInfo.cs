using EmployeeSystem.Contract.Enums;

namespace EmployeeSystem.Contract.Dtos.Info
{
    public class EmployeeInfo
    {
        public int? Id { get; set; }

        public required string Name { get; set; }

        public string? DepartmentName { get; set; }

        public string? ManagerName { get; set; }

        public Role Role { get; set; } = Role.Employee;

        public decimal Salary { get; set; }
        
        public required string Email { get; set; }

        public required string Address { get; set; }

        public required string ImageUrl { get; set; }

        public required string Phone { get; set; }

        public required string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
