using EmployeeSystem.Contract.Dtos;
using EmployeeSystem.Contract.Interfaces;
using EmployeeSystem.Contract.Models;
using Microsoft.EntityFrameworkCore;
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
        public async Task<bool> Pay(int employeeId)
        {
            try
            {
                var lastPaid = await _context.Salaries
                    .OrderByDescending(s => s.Id).FirstOrDefaultAsync(s => s.EmployeeId == employeeId);
                var curr = DateTime.Now;

                var salaryPay = new Salary
                {
                    EmployeeId = employeeId,
                    Status = SalaryStatus.Paid,
                    PaidOn = DateTime.Now
                };
                if (lastPaid != null)
                {

                    var month = (curr.Month - lastPaid.PaidOn.Month);

                    if (lastPaid.Status == SalaryStatus.AdvancePaid)
                    {
                        if(month == 1)
                        {
                            salaryPay.Status = SalaryStatus.AdvancePaid;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    
                }

                _context.Salaries.Add(salaryPay);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<bool> PayAll()
        {
            try
            {
                // extract all the unpaid employees

                // pay them


                return true;
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
