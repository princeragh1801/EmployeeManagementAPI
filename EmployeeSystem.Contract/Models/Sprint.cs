using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeSystem.Contract.Models
{
    public class Sprint : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ?projectId { get; set; }

        [ForeignKey(nameof(projectId))]
        public Project Project { get; set; }
        public bool isActive { get; set; } = true;
    }
}
