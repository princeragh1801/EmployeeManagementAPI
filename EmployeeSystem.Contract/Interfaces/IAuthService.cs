using EmployeeSystem.Contract.Dtos;

namespace EmployeeSystem.Contract.Interfaces
{
    public interface IAuthService
    {
        /// <summary>
        /// Generates a JWT token for the specified user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="emp">The employee data transfer object containing user details.</param>
        /// <returns>
        /// A JWT token as a string.
        /// </returns>
        public string GeneratingToken(int userId, EmployeeDto emp);


        //public Task<User?> Register(int userId, RegisterDto userDto);

        /// <summary>
        /// Checks the user's credentials and returns the user information if the credentials match.
        /// </summary>
        /// <param name="userDto">An object containing the user's login credentials.</param>
        /// <returns>
        /// A task representing the asynchronous operation, containing a LoginUserDto if the credentials are valid, or null if they are not.
        /// </returns>
        public Task<LoginUserDto?> Login(RegisterDto userDto);


        //public Task<bool> RegisterMany(int userId, List<RegisterDto> list);
    }
}
