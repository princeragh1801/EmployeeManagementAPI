using EmployeeSystem.Contract.Enums;

namespace EmployeeSystem.Contract.Dtos
{
    public class TaskBasicDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TasksStatus Status { get; set; }
        public TaskType TaskType { get; set; }
    }
}
