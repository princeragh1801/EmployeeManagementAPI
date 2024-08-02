using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeSystem.Contract.Dtos
{
    public class AddTaskReviewDto
    {
        [Required(ErrorMessage = "Task id is required")]
        [Range(1, int.MaxValue)]
        public int TaskID { get; set; }

        [Required(ErrorMessage = "Content is required")]
        [MinLength(2)]
        public string Content { get; set; }

        [Required(ErrorMessage = "Reviewer id is required") ]
        [Range(1, int.MaxValue)]
        public int ReviewerId { get; set; }
    }
}
