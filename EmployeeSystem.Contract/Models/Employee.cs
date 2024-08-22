using System.ComponentModel.DataAnnotations.Schema;
using static EmployeeSystem.Contract.Enums.Enums;

namespace EmployeeSystem.Contract.Models
{
    public class Employee : BaseEntity
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public string ImageUrl { get; set; }

        public decimal Salary { get; set; }

        public int? DepartmentID { get; set; }

        public int? ManagerID { get; set; }

        public  Role Role{ get; set; }

        [ForeignKey(nameof(ManagerID))]
        public virtual Employee? Manager { get; set; }

        [ForeignKey(nameof(DepartmentID))]
        public virtual Department? Department { get; set; }

        public bool IsActive { get; set; } = true;
       
    }
}
