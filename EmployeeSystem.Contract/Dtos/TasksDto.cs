using static EmployeeSystem.Contract.Enums.Enums;

namespace EmployeeSystem.Contract.Dtos
{
    public class TasksDto
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TasksStatus Status { get; set; }
/*        public int AssignedBy { get; set; }
        public int AssignedTo { get; set; }*/
        public int? ProjectId { get; set; }
        public TaskType TaskType { get; set; }
        public string ?AssignerName { get; set; }
        public string ?AssigneeName { get; set; }
        public DateTime CreatedOn { get; set; }
        
        /*public EmployeeDto? AssignerDetails { get; set; }
        public EmployeeDto? AssigneeDetails { get; set; }
        public ProjectDetailsDto? ProjectDetails { get; set; }
        public List<TaskReviewDto> ?Reviews { get; set; }*/
    }
}
