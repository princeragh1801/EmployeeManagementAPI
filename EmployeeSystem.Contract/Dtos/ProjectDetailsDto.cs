using EmployeeSystem.Contract.Enums;

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

        public List<ProjectEmployeeDto> Members { get; set; }

        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int ActiveTasks { get; set; }
        public int PendingTasks { get; set; }
    }
}
