using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EmployeeSystem.Contract.Enums.Enums;

namespace EmployeeSystem.Contract.Dtos.Info.PaginationInfo
{
    public class ProjectTasksDto : PaginatedDto
    {
        public List<TaskType>? Types { get; set; }

        public List<TasksStatus>? Status { get; set; }

        public bool? Assign { get; set; }

        public List<int>? AssignedTo { get; set; }

        public int? SprintId { get; set; }
    }
}
