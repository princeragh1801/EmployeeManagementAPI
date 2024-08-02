using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EmployeeSystem.Contract.Enums.Enums;


namespace EmployeeSystem.Contract.Dtos
{
    public class SalaryDto
    {
        public int Id { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int EmployeeId { get; set; }

        public DateOnly Date { get; set; }

        public SalaryStatus Status { get; set; }
    }
}
