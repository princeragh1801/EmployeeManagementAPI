using EmployeeSystem.Contract.Interfaces;
using EmployeeSystem.Contract.Models;
using Microsoft.EntityFrameworkCore;
using static EmployeeSystem.Contract.Enums.Enums;


namespace EmployeeSystem.Provider.Services
{
    public class RequestService : IRequestService
    {
        private readonly ApplicationDbContext _context;
        private readonly ISalaryService _salaryService;

        public RequestService(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public async Task<bool> RespondRequest(int userId, int id, Request reqDto)
        {
            try
            {

                var request = await _context.Requests.FirstOrDefaultAsync(r => r.Id == id);
                if (request == null || request.RequestStatus != RequestStatus.Requested)
                {
                    return false;
                }
                request.UpdatedOn = DateTime.Now;
                request.UpdatedBy = userId;
                if (request.RequestType == RequestType.AdvanceSalary)
                {
                    var salaryDetails = await _context.Salaries.LastOrDefaultAsync(s => s.EmployeeId == reqDto.RequestedBy);
                    if (salaryDetails != null)
                    {
                        if(salaryDetails.Status == SalaryStatus.AdvancePaid)
                        {
                            /*var checkEligibleAndPay = a _salaryService.Pay(reqDto.RequestedBy);
                            if (checkEligibleAndPay != null && checkEligibleAndPay)
                            {

                            }
                            else
                            {
                                request.RequestStatus = RequestStatus.Rejected;
                                return false;
                            }*/
                            return true;
                        }
                        else
                        {
                            await _salaryService.Pay(request.RequestedBy);
                            request.RequestStatus = RequestStatus.Approved;
                            return true;
                        }
                        
                    }
                }
                return true;
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
