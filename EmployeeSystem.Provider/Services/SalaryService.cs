using EmployeeSystem.Contract.Dtos;
using EmployeeSystem.Contract.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EmployeeSystem.Contract.Enums.Enums;

namespace EmployeeSystem.Provider.Services
{
    public class SalaryService : ISalaryService
    {
        private readonly ApplicationDbContext _context;

        public SalaryService(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public async Task<List<SalaryDto>> GetEmployeeSalaryDetails(int employeeId)
        {
            try
            {
                var salaryDetails = await _context.Salaries
                    .Where(s => s.EmployeeId == employeeId)
                    .Select(s => new SalaryDto
                    {
                        Id = s.Id,
                        Status = s.Status
                    }).ToListAsync();

                return salaryDetails;
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // TODO :
        public async Task<int> AddSalary(int employeeId)
        {
            try
            {
                var lastPaid = await _context.Salaries
                    .LastOrDefaultAsync(s => s.EmployeeId == employeeId);

                if (lastPaid != null && lastPaid.Status == SalaryStatus.Paid) return 0;
                return 1;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
