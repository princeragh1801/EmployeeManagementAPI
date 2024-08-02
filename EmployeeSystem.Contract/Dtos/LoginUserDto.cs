namespace EmployeeSystem.Contract.Dtos
{
    public class LoginUserDto
    {
        public EmployeeDto Employee { get; set; }
        public string Token { get; set; }
    }
}
