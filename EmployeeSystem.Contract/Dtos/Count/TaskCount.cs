namespace EmployeeSystem.Contract.Dtos.Count
{
    public class TaskCount
    {
        public TaskTypeCount TypeCount { get; set; }
        public TaskStatusCount StatusCount { get; set; }
        public AssignCount AssignCount { get; set; }
    }
}
