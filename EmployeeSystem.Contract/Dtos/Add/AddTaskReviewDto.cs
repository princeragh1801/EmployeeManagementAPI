using System.ComponentModel.DataAnnotations;

namespace EmployeeSystem.Contract.Dtos.Add
{
    public class AddTaskReviewDto
    {

        [Required(ErrorMessage = "Content is required")]
        [MinLength(2)]
        public required string Content { get; set; }

        /*[Required(ErrorMessage = "Reviewer id is required") ]
        [Range(1, int.MaxValue)]
        public int ReviewerId { get; set; }*/
    }
}
