using EmployeeSystem.Contract.Dtos;
using EmployeeSystem.Contract.Interfaces;
using EmployeeSystem.Contract.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeSystemWebApi.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    [Route("[controller]")]
    [ApiController]
    public class SalaryController : ControllerBase
    {
        private readonly ISalaryService _salaryService;

        public SalaryController(ISalaryService salaryService)
        {
            _salaryService = salaryService;
        }

        [HttpGet("{employeeId}")]
        public async Task<ActionResult<ApiResponse<List<SalaryDto>>>> Get(int employeeId)
        {
            try
            {
                var salaryDetails = await _salaryService.GetEmployeeSalaryDetails(employeeId);

                var response = new ApiResponse<List<SalaryDto>>
                {
                    Success = true,
                    Status = 200,
                    Message = "Attendance fetched",
                    Data = salaryDetails
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ApiResponse<List<SalaryDto>>
                {
                    Success = false,
                    Status = 500,
                    Message = ex.Message,
                };
                return BadRequest(response);
            }
        }

        [HttpPost("{employeeId}")]
        public async Task<ActionResult<ApiResponse<bool>>> Pay(int employeeId)
        {
            try
            {
                var response = new ApiResponse<bool>();
                var paid = await _salaryService.Pay(employeeId);

                if (!paid)
                {
                    response.Message = "Already taken advance";
                    return Conflict(response);
                }
                response.Message = "Salary paid";
                return Ok(response);
            }catch(Exception ex)
            {
                var response = new ApiResponse<bool>();
                response.Message = ex.Message;
                response.Status = 500;
                response.Success = false;
                return BadRequest(response);
            }
        }
    }
}
