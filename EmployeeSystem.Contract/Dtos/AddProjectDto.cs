using System.ComponentModel.DataAnnotations;

namespace EmployeeSystem.Contract.Dtos
{
    public class AddProjectDto
    {
        [Required(ErrorMessage = "Name is required")]
        [MinLength(1)]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [MinLength(5)]
        public string Description { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int AdminId { get; set; }

        [Required]
        public List<AddProjectEmployeeDto> Members { get; set; }
    }
}
