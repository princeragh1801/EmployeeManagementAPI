using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeSystemWebApi.Contract.Models
{
    public class Tasks
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        [Required]
        public int AssignedTo { get; set; }
        [Required]
        public int AsssignedBy { get; set; }

        [ForeignKey(nameof(AssignedTo))]
        public Employee Employee { get; set; }

        [ForeignKey(nameof(AsssignedBy))]
        public Employee Admin { get; set; }
    }
}
