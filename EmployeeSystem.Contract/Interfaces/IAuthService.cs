using EmployeeSystem.Contract.Dtos;
using EmployeeSystem.Contract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
