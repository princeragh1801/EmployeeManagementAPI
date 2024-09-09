using EmployeeSystem.Contract.Dtos.Count;

namespace EmployeeSystem.Contract.Dtos.Info.PaginationInfo
{
    public class TaskPaginationInfo
    {
        public List<TasksDto> Tasks { get; set; }
        public TaskCount Count { get; set; }
    }
}
