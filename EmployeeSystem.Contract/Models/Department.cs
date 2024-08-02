using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeSystem.Contract.Models
{
    public class Department : BaseEntity
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; } = true;

        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
