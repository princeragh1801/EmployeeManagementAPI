using System.ComponentModel.DataAnnotations;

namespace EmployeeSystem.Contract.Dtos
{
    public class UserDto
    {
        public int Id { get; set; } = 0;

        [Required, MinLength(5)]
        public string Username { get; set; }

        [Required, MinLength(5)]
        public string Password { get; set; }

        public string ?Token { get; set; }
    }
}
