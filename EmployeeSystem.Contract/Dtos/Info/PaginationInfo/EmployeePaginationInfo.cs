using static EmployeeSystem.Contract.Enums.Enums;

namespace EmployeeSystem.Contract.Dtos.Info.PaginationInfo
{
    public class EmployeePaginationInfo
    {
        public int? Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string? DepartmentName { get; set; }

        public string? ManagerName { get; set; }

        public Role Role { get; set; }

        public decimal Salary { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
