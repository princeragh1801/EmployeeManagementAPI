using EmployeeSystem.Contract.Enums;

namespace EmployeeSystem.Contract.Dtos.Info.PaginationInfo
{
    public class ProjectTasksDto : PaginatedDto<List<TasksStatus>?>
    {
        public List<TaskType>? Types { get; set; }

        public bool? Assign { get; set; }

        public List<int>? AssignedTo { get; set; }

        public int? SprintId { get; set; }
    }
}
