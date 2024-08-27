using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EmployeeSystem.Contract.Enums.Enums;

namespace EmployeeSystem.Contract.Dtos.Info.PaginationInfo
{
    public class DepartmentPaginationInfo
    {
        public int? Id { get; set; }

        public required string Name { get; set; }

        public required string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
