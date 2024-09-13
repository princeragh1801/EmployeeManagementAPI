using EmployeeSystem.Contract.Dtos.Info;

namespace EmployeeSystem.Contract.Dtos
{
    public class LoginUserDto
    {
        public EmployeeLoginInfo Employee { get; set; }
        public string Token { get; set; }
    }
}
