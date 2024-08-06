using EmployeeSystem.Contract.Dtos;
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

        public RequestService(ApplicationDbContext applicationDbContext, ISalaryService salaryService)
        {
            _context = applicationDbContext;
            _salaryService = salaryService; 
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
                    var salaryDetails = await _context.Salaries
                        .OrderByDescending(s => s.Id)
                        .LastOrDefaultAsync(s => s.EmployeeId == reqDto.RequestedBy);

                    if (salaryDetails != null)
                    {
                        if(salaryDetails.Status == SalaryStatus.AdvancePaid)
                        {
                            var checkEligibleAndPay = await _salaryService.Pay(reqDto.RequestedBy);
                            if (checkEligibleAndPay)
                            {
                                request.RequestStatus = RequestStatus.Approved;
                                return true;
                            }

                            request.RequestStatus = RequestStatus.Rejected;
                            return false;
                            
                        }
                        
                        await _salaryService.Pay(request.RequestedBy);
                        request.RequestStatus = RequestStatus.Approved;
                        return true;
                    }
                    await _salaryService.Pay(reqDto.RequestedBy);
                    request.RequestStatus = RequestStatus.Approved;
                    return true;
                }
                return false;
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        // from here we can generate the number of notifications
        public async Task<List<RequestDto>> GetAllPendingRequest()
        {
            try
            {
                var query = _context.Requests
                    .Include(r => r.Employee)
                    .Where(r => r.RequestStatus == RequestStatus.Requested);

                var totalRequest = query.Count();
                var pendingRequests = await query
                    .Select(r => new RequestDto
                    {
                        Id = r.Id,
                        Name = r.Employee.Name,
                        RequestedBy = r.Employee.Id,
                        Status = r.RequestStatus,
                        CreatedOn = r.CreatedOn,
                        TotalPendingRequests = totalRequest,

                    }).ToListAsync();

                return pendingRequests;

            }catch( Exception ex)
            {
                throw new Exception(ex.Message );
            }
        }



    }
}
