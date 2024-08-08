using System.ComponentModel.DataAnnotations;

namespace EmployeeSystem.Contract.Dtos.Add
{
    public class AddProjectEmployeeDto
    {
        [Required(ErrorMessage = "Id is required")]
        [Range(1, int.MaxValue)]
        public int EmployeeId { get; set; }
    }
}
