using EmployeeSystemWebApi.Contract.Enums;
using System.ComponentModel.DataAnnotations;

namespace EmployeeSystemWebApi.Contract.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Salary { get; set; }
        public int? DepartmentID { get; set; }
        public int? ManagerID { get; set; }
        [Required]
        public Role Role { get; set; }

        public Employee ?Manager { get; set; }
        public Department ?Department { get; set; }
    }
}
