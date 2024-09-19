using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace EmployeeSystem.Provider.Services
{
    public class Authservice : IAuthService
    {
        private readonly ApplicationDbContext _context;

        public Authservice(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public string GeneratingToken(Employee emp)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY")));
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, emp.Name),   
                new Claim("Name", emp.Name),
                new Claim("UserId", emp.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, emp.Role.ToString()),
                new Claim("Role", emp.Role.ToString()),
                new Claim("Date", DateTime.Now.ToString()),
                
            };


            var token = new JwtSecurityToken(
                    Environment.GetEnvironmentVariable("JWT_ISSUER"),
                    Environment.GetEnvironmentVariable("JWT_AUDIENCE"),
                    claims: claims,
                    expires: DateTime.Now.AddDays(5),
                    signingCredentials: credential
                );

            return tokenHandler.WriteToken(token);
        }

        public async Task<LoginUserDto?> Login(RegisterDto userDto)
        {
            try
            {
                string userName = userDto.Username;
                string password = userDto.Password;

                var employee = await _context.Employees.FirstOrDefaultAsync(u => u.Username == userName);

                if (employee == null)
                {
                    return null;
                }
                if (employee.Password != password)
                {
                    return null;
                }

                var isManager = await _context.Employees.FirstOrDefaultAsync(e => e.ManagerID == employee.Id);
                var token = GeneratingToken(employee);

                var employeeInfo = new EmployeeLoginInfo
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Role = employee.Role,
                    IsManager = isManager != null
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


    }
}
