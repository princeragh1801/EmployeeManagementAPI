using EmployeeSystem.Contract.Dtos;

namespace EmployeeSystem.Contract.Interfaces
{
    public interface IAuthService
    {
        /// <summary>
        /// This function is used for generating the jwt token
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="emp"></param>
        /// <returns></returns>
        public string GeneratingToken(int userId, EmployeeDto emp);

        //public Task<User?> Register(int userId, RegisterDto userDto);

        /// <summary>
        /// This function just checks the users credentials. 
        /// In response it returns the user or null according the credentials matched or not
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        public Task<LoginUserDto?> Login(RegisterDto userDto);

        //public Task<bool> RegisterMany(int userId, List<RegisterDto> list);
    }
}
