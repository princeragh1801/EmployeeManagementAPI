using System.ComponentModel.DataAnnotations.Schema;
using static EmployeeSystem.Contract.Enums.Enums;

namespace EmployeeSystem.Contract.Models
{
    public class Employee : BaseEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public string ImageUrl { get; set; }

        public decimal Salary { get; set; }

        public int? DepartmentID { get; set; }

        public int? ManagerID { get; set; }

        public int UserId { get; set; }

        public  Role Role{ get; set; }

        public virtual Employee? Manager { get; set; }

        public virtual Department? Department { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        public bool IsActive { get; set; } = true;

        public virtual ICollection<ProjectEmployee> ProjectEmployees { get; set; } = new List<ProjectEmployee>();

        public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

       
    }
}
