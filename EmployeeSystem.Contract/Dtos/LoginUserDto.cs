using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeSystem.Contract.Dtos
{
    public class LoginUserDto
    {
        public EmployeeDto Employee { get; set; }
        public string Token { get; set; }
    }
}
