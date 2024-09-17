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
                // fetching the salary details of the employee
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


        // CHECK
        public async Task<bool> Pay(int employeeId)
        {
            try
            {
                // fetching the last salary paid details
                var lastPaid = await _context.Salaries
                    .OrderByDescending(s => s.Id).FirstOrDefaultAsync(s => s.EmployeeId == employeeId);
                var curr = DateTime.Now;

                // creating a new salary instance model
                var salaryPay = new Salary
                {
                    EmployeeId = employeeId,
                    Status = SalaryStatus.Paid,
                    PaidOn = DateTime.Now
                };
                if (lastPaid != null)
                {
                    // calculating the month difference
                    var month = (curr.Month - lastPaid.PaidOn.Month);

                    // if salary is paid in advance
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

        // TODO :
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
