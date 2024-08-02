using EmployeeSystem.Contract.Dtos;

namespace EmployeeSystem.Contract.Interfaces
{
    public interface IAuthService
    {
        public string GeneratingToken(int userId, EmployeeDto emp);

        //public Task<User?> Register(int userId, RegisterDto userDto);

        public Task<LoginUserDto?> Login(RegisterDto userDto);

        //public Task<bool> RegisterMany(int userId, List<RegisterDto> list);
    }
}
