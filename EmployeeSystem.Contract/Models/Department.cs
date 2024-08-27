namespace EmployeeSystem.Contract.Models
{
    public class Department : BaseEntity
    {

        public int Id { get; set; }

        public required string Name { get; set; }

        public bool IsActive { get; set; } = true;

        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
