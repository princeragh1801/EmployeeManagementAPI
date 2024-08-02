using System.ComponentModel.DataAnnotations;

namespace EmployeeSystem.Contract.Dtos
{
    public class RegisterDto
    {
        [Required]
        [MinLength(6)]
        public string Username { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }
}
