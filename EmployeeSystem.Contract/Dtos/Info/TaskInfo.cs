using EmployeeSystem.Contract.Dtos.IdAndName;

namespace EmployeeSystem.Contract.Dtos.Info
{
    public class TaskInfo
    {
        public TasksDto Task { get; set; }
        public List<TaskReviewDto> ?Reviews { get; set; }
        public List<TaskIdAndName> ?SubTasks { get; set; }
    }
}
