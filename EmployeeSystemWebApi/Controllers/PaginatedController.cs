using EmployeeSystem.Contract.Dtos;
using EmployeeSystem.Contract.Interfaces;
using EmployeeSystem.Contract.Response;
using EmployeeSystem.Provider.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeSystemWebApi.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class PaginatedController : ControllerBase
    {
        private readonly IPaginatedService _paginatedService;

        public PaginatedController(IPaginatedService paginatedService)
        {
            _paginatedService = paginatedService;
        }

        [HttpPost("employees")]
        public async Task<ActionResult<ApiResponse<List<EmployeeDto>>>> GetEmployees(PaginatedDto paginatedDto)
        {
            try
            {
                var employees = await _paginatedService.GetEmployees(paginatedDto);

                var response = new ApiResponse<List<EmployeeDto>>
                {
                    Success = true,
                    Status = 200,
                    Message = "Employees details fetched",
                    Data = employees
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<List<EmployeeDto>>
                {
                    Success = false,
                    Status = 500,
                    Message = ex.Message,
                    Data = null
                });
            }
        }[HttpPost("departments")]
        public async Task<ActionResult<ApiResponse<List<EmployeeDto>>>> GetDepartments(PaginatedDto paginatedDto)
        {
            try
            {
                var departments = await _paginatedService.GetDepartments(paginatedDto);

                var response = new ApiResponse<List<DepartmentDto>>
                {
                    Success = true,
                    Status = 200,
                    Message = "Employees details fetched",
                    Data = departments
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<List<DepartmentDto>>
                {
                    Success = false,
                    Status = 500,
                    Message = ex.Message,
                    Data = null
                });
            }
        }
    }
}
