using System.ComponentModel.DataAnnotations;

namespace EmployeeSystem.Contract.Dtos
{
    public class AddDepartmentDto
    {
        [Required(ErrorMessage = "Name is required")]
        [MinLength(2)]
        public string Name { get; set; }
    }
}
