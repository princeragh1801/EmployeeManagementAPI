using System.ComponentModel.DataAnnotations;

namespace EmployeeSystem.Contract.Dtos.Add
{
    public class AddSprintDto
    {
        [Required]
        [MinLength(2)]
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ?ProjectId { get; set; }
    }
}
