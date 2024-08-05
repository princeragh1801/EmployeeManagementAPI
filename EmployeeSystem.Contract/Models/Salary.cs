using System.ComponentModel.DataAnnotations.Schema;
using static EmployeeSystem.Contract.Enums.Enums;

namespace EmployeeSystem.Contract.Models
{
    public class Salary
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; } 

        public SalaryStatus Status { get; set; } = SalaryStatus.Paid;

        public DateTime PaidOn { get; set; } = DateTime.Now;

        [ForeignKey(nameof(EmployeeId))]
        public Employee Employee { get; set; }

    }
}
