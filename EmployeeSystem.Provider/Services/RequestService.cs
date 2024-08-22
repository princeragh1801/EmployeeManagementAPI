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
                // fetching the request to respond
                var request = await _context.Requests.FirstOrDefaultAsync(r => r.Id == id);
                if (request == null || request.RequestStatus != RequestStatus.Requested)
                {
                    return false;
                }

                // updating the request
                request.UpdatedOn = DateTime.Now;
                request.UpdatedBy = userId;

                // checking the type
                if (request.RequestType == RequestType.AdvanceSalary)
                {
                    // extracting the last paid details
                    var salaryDetails = await _context.Salaries
                        .OrderByDescending(s => s.Id)
                        .LastOrDefaultAsync(s => s.EmployeeId == reqDto.CreatedBy);

                    if (salaryDetails != null)
                    {
                        // if last paid is advance
                        if(salaryDetails.Status == SalaryStatus.AdvancePaid)
                        {
                            // check for the employee is eligible for the pay
                            var checkEligibleAndPay = await _salaryService.Pay(reqDto.CreatedBy??0);
                            if (checkEligibleAndPay)
                            {
                                // approved:: updating the status
                                request.RequestStatus = RequestStatus.Approved;
                                return true;
                            }
                            // rejected ::
                            request.RequestStatus = RequestStatus.Rejected;
                            return false;
                            
                        }
                        // approved ::
                        await _salaryService.Pay(request.CreatedBy ?? 0);
                        request.RequestStatus = RequestStatus.Approved;
                        return true;
                    }
                    await _salaryService.Pay(reqDto.CreatedBy ?? 0);
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
                // creating a query for all the request which are pending right now
                var query = _context.Requests
                    .Include(r => r.Creator)
                    .Where(r => r.RequestStatus == RequestStatus.Requested);

                // creating a total count variable and storing the count of pending requests
                var totalRequest = query.Count();

                // creating dto of all the pending request
                var pendingRequests = await query
                    .Select(r => new RequestDto
                    {
                        Id = r.Id,
                        Name = r.Creator.Name,
                        RequestedBy = r.Creator.Id,
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
