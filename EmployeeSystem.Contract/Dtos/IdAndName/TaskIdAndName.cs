using static EmployeeSystem.Contract.Enums.Enums;

namespace EmployeeSystem.Contract.Dtos.IdAndName
{
    public class TaskIdAndName
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TaskType TaskType { get; set; }
    }
}
