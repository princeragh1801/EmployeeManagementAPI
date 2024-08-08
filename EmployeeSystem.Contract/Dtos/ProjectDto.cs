
using static EmployeeSystem.Contract.Enums.Enums;

namespace EmployeeSystem.Contract.Dtos
{
    public class ProjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ProjectStatus Status { get; set; } = ProjectStatus.Pending;
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        //public List<AddProjectEmployeeDto> ProjectEmployees { get; set; }

        //public List<TasksDto> Tasks { get; set; }

    }
}
