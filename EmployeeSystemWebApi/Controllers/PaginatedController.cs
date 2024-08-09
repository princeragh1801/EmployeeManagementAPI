using EmployeeSystem.Contract.Dtos;
using EmployeeSystem.Contract.Dtos.Info.PaginationInfo;
using EmployeeSystem.Contract.Interfaces;
using EmployeeSystem.Contract.Response;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<ActionResult<ApiResponse<PaginatedItemsDto<List<EmployeePaginationInfo>>>>> GetEmployees(PaginatedDto paginatedDto)
        {
            try
            {
                var userId = Convert.ToInt32(HttpContext.User.Claims.First(e => e.Type == "UserId")?.Value);
                var employees = await _paginatedService.GetEmployees(userId, paginatedDto);

                var response = new ApiResponse<PaginatedItemsDto<List<EmployeePaginationInfo>>>
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
                return BadRequest(new ApiResponse<List<EmployeePaginationInfo>>
                {
                    Success = false,
                    Status = 500,
                    Message = ex.Message,
                    Data = null
                });
            }
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost("departments")]
        public async Task<ActionResult<ApiResponse<PaginatedItemsDto<List<DepartmentPaginationInfo>>>>> GetDepartments(PaginatedDto paginatedDto)
        {
            try
            {
                var departments = await _paginatedService.GetDepartments(paginatedDto);

                var response = new ApiResponse<PaginatedItemsDto<List<DepartmentPaginationInfo>>>
                {
                    Success = true,
                    Status = 200,
                    Message = "Departments fetched",
                    Data = departments
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<List<DepartmentPaginationInfo>>
                {
                    Success = false,
                    Status = 500,
                    Message = ex.Message,
                    Data = null
                });
            }
        }
        
        [HttpPost("projects")]
        public async Task<ActionResult<ApiResponse<PaginatedItemsDto<List<ProjectDto>>>>> GetProjects(PaginatedDto paginatedDto)
        {
            try
            {
                var userId = Convert.ToInt32(HttpContext.User.Claims.First(e => e.Type == "UserId")?.Value);
                var projects = await _paginatedService.GetProjects(userId, paginatedDto);

                var response = new ApiResponse<PaginatedItemsDto<List<ProjectDto>>>
                {
                    Success = true,
                    Status = 200,
                    Message = "Projects fetched",
                    Data = projects
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<List<ProjectDto>>
                {
                    Success = false,
                    Status = 500,
                    Message = ex.Message,
                    Data = null
                });
            }
        }


        [HttpPost("tasks")]
        public async Task<ActionResult<ApiResponse<PaginatedItemsDto<List<TasksDto>>>>> GetTasks(PaginatedDto paginatedDto)
        {
            try
            {
                var userId = Convert.ToInt32(HttpContext.User.Claims.First(e => e.Type == "UserId")?.Value);
                var tasks = await _paginatedService.GetTasks(userId, paginatedDto);

                var response = new ApiResponse<PaginatedItemsDto<List<TasksDto>>>
                {
                    Success = true,
                    Status = 200,
                    Message = "Tasks fetched",
                    Data = tasks
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<List<TasksDto>>
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
