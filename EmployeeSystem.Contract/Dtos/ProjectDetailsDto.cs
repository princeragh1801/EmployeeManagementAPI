using static EmployeeSystem.Contract.Enums.Enums;

namespace EmployeeSystem.Contract.Dtos
{
    public class ProjectDetailsDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ProjectStatus Status { get; set; } = ProjectStatus.Pending;

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public List<TaskBasicDto> Tasks { get; set; }

        public List<ProjectEmployeeDto> Members { get; set; }
    }
}
