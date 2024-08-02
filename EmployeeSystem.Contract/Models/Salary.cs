using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EmployeeSystem.Contract.Enums.Enums;

namespace EmployeeSystem.Contract.Models
{
    public class Salary
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; } 

        public SalaryStatus Status { get; set; } = SalaryStatus.Paid;

        [ForeignKey(nameof(EmployeeId))]
        public Employee Employee { get; set; }

    }
}
