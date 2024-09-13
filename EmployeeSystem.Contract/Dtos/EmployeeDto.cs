using EmployeeSystem.Contract.Enums;
namespace EmployeeSystem.Contract.Dtos
{
    public class EmployeeDto : BaseEntityDto
    {
        public int ?Id { get; set; }

        public required string Name { get; set; }
        public string? DepartmentName { get; set; }
        public string? ManagerName { get; set; }
        public  Role Role { get; set; } = Role.Employee;

        public decimal Salary { get; set; }
        public int? DepartmentId { get; set; }
        public int? ManagerId { get; set; }
    }
}
