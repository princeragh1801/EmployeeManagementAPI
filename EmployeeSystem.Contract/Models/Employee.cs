using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeSystem.Contract.Models
{
    public class Employee : BaseEntity
    {
        public int Id { get; set; }

        public required string Username { get; set; }

        public required string Password { get; set; }

        public required string Name { get; set; }

        public required string Email { get; set; }

        public required string Phone { get; set; }

        public required string Address { get; set; }

        public required string ImageUrl { get; set; }

        public decimal Salary { get; set; }

        public int? DepartmentID { get; set; }

        public int? ManagerID { get; set; }

        public Role Role { get; set; }

        [ForeignKey(nameof(ManagerID))]
        public virtual Employee? Manager { get; set; }

        [ForeignKey(nameof(DepartmentID))]
        public virtual Department? Department { get; set; }

        public bool IsActive { get; set; } = true;

    }
}
