using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeSystem.Contract.Models
{
    public class ProjectEmployee
    {
        
        public int ProjectId { get; set; }
        
        public int EmployeeId { get; set; }

        [ForeignKey(nameof(ProjectId))]
        public Project Project { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public Employee Employee { get; set; }
    }
}
