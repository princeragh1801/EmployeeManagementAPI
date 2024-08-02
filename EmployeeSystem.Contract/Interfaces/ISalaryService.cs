using EmployeeSystem.Contract.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeSystem.Contract.Interfaces
{
    public interface ISalaryService
    {
        public Task<List<SalaryDto>> GetEmployeeSalaryDetails(int id);

        public Task<int> AddSalary(int employeeId);
    }
}
