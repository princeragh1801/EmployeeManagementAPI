using EmployeeSystem.Contract.Dtos;
using EmployeeSystem.Contract.Interfaces;
using EmployeeSystem.Contract.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeSystemWebApi.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;

        public AttendanceController(IAttendanceService attendance)
        {
            _attendanceService = attendance;
        }

        [HttpGet("{employeeId}")]
        public async Task<ActionResult<ApiResponse<List<AttendanceDto>>>> Get(int employeeId)
        {
            try
            {
                var response = await _attendanceService.GetByEmployeeId(employeeId);

                if(response.Status == 404)
                {
                    return NotFound(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ApiResponse<List<AttendanceDto>>
                {
                    Success = false,
                    Status = 500,
                    Message = ex.Message,
                };
                return BadRequest(response);
            }
        }

        [HttpPost("{employeeId}")]
        public async Task<ActionResult<ApiResponse<int>>> Add(int employeeId)
        {
            try
            {
                var response = await _attendanceService.Add(employeeId);
                
                return Ok(response);
            }catch(Exception ex)
            {
                var response = new ApiResponse<int>
                {
                    Success = false,
                    Status = 500,
                    Message = ex.Message,
                };
                return BadRequest(response);
            }
        }
    }
}
