using EmployeeSystem.Contract.Dtos;
using EmployeeSystem.Contract.Dtos.IdAndName;
using EmployeeSystem.Contract.Dtos.Info;
using EmployeeSystem.Contract.Interfaces;
using EmployeeSystem.Contract.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EmployeeSystem.Provider.Services
{
    public class Authservice : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration configuration;

        public Authservice(ApplicationDbContext applicationDbContext, IConfiguration _configuration)
        {
            _context = applicationDbContext;
            this.configuration = _configuration;
        }

        public string GeneratingToken(int userId, Employee emp)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, emp.Name),
                new Claim("Id", userId.ToString()),
                new Claim("UserId", emp.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, emp.Role.ToString())
                
            };


            var token = new JwtSecurityToken(
                    configuration["Jwt:Issuer"],
                    configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddDays(5),
                    signingCredentials: credential
                );

            return tokenHandler.WriteToken(token);
        }


        public async Task<User?> Register(int userId, RegisterDto userDto)
        {
            try
            {

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == userDto.Username);

                if (user != null)
                {
                    return null;
                }

                var newUser = new User
                {
                    Username = userDto.Username,
                    Password = userDto.Password,
                    CreatedBy = userId,
                    CreatedOn = DateTime.Now,
                };
                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                return newUser;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<LoginUserDto?> Login(RegisterDto userDto)
        {
            try
            {
                string userName = userDto.Username;
                string password = userDto.Password;

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == userName);

                if (user == null)
                {
                    return null;
                }
                if (user.Password != password)
                {
                    return null;
                }
                var employee = await _context.Employees.Include(e => e.User).FirstOrDefaultAsync(e=> e.User.Id == user.Id & e.IsActive);

                if (employee == null)
                {
                    return null;
                }

                var token = GeneratingToken(user.Id, employee);

                var employeeInfo = new EmployeeLoginInfo
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Role = employee.Role,
                };
                var data = new LoginUserDto
                {
                    Token = token,
                    Employee = employeeInfo
                };



                return data;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> RegisterMany(int userId, List<RegisterDto> list)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await transaction.CreateSavepointAsync("Adding list");
                foreach (var userDto in list)
                {
                    var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == userDto.Username);

                    if (user != null)
                    {
                        return false;
                    }

                    var newUser = new User
                    {
                        Username = userDto.Username,
                        Password = userDto.Password,
                        CreatedBy = userId,
                        CreatedOn = DateTime.Now,
                    };
                    _context.Users.Add(newUser);
                }
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackToSavepointAsync("Adding list");
                throw new Exception(ex.Message);
            }
        }


    }
}
