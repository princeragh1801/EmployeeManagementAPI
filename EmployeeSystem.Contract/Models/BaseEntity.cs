using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeSystem.Contract.Models
{
    public class BaseEntity
    {
        public int CreatedBy { get; set; }
        public int ?UpdatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ?UpdatedOn { get; set; }
    }
}
