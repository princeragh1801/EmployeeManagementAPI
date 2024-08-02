using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeSystemWebApi.Contract.Models
{
    public class TaskReview
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int TaskID { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public DateTime ReviewedAt { get; set; }
        [Required]
        public int ReviewedBy { get; set; }

        [ForeignKey(nameof(TaskID))]
        public Tasks Task { get; set; }
        [ForeignKey(nameof(ReviewedBy))]
        public Employee Reviewer { get; set; }

    }
}
