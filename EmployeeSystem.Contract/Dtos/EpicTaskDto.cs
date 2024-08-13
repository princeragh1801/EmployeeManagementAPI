using static EmployeeSystem.Contract.Enums.Enums;

namespace EmployeeSystem.Contract.Dtos
{
    public class EpicTaskDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TaskType TaskType { get; set; }
        public DateTime CreatedOn { get; set; }
        public TasksStatus Status { get; set; }
        public string ?AssignedTo { get; set; }
        public List<EpicTaskDto> ?SubItems { get; set; }
    }
}
