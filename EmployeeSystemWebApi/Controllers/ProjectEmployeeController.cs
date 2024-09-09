using EmployeeSystem.Contract.Dtos.IdAndName;
using EmployeeSystem.Contract.Interfaces;
using EmployeeSystem.Contract.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeSystemWebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectEmployeeController : ControllerBase
    {
        private readonly IProjectEmployeeService _employeeService;
        public ProjectEmployeeController(IProjectEmployeeService projectEmployeeService)
        {
            _employeeService = projectEmployeeService;
        }

        [HttpGet("{projectId}")]
        public async Task<ActionResult<ApiResponse<List<EmployeeIdAndName>>>> GetAll(int projectId)
        {
            try
            {
                var response = new ApiResponse<List<EmployeeIdAndName>>();
                var employees = await _employeeService.GetAll(projectId);
                response.Data = employees;
                response.Message = "Employees fetched";
                return Ok(response);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{projectId}")]
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<ActionResult<ApiResponse<bool>>> AddMembers(int projectId, List<int> employeesToAdd)
        {
            try
            {
                var claims = HttpContext.User.Claims;
                var add = await _employeeService.AddMembers(projectId, claims, employeesToAdd);
                if (!add)
                {
                    return BadRequest("Error occured while adding");
                }
                var response = new ApiResponse<bool>
                {
                    Success = true,
                    Status = 200,
                    Message = "Members added",
                    Data = true
                };

                /*if (id == 0)
                {
                    response.Message = "Unauthorized request";
                }*/

                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ApiResponse<int>
                {
                    Success = false,
                    Status = 500,
                    Message = ex.Message
                };
                return BadRequest(response);
            }

        }

        [HttpDelete("{projectId}")]
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteMembers(int projectId, List<int> employeesToDelete)
        {
            try
            {
                var claims = HttpContext.User.Claims;
                var add = await _employeeService.DeleteMembers(projectId, claims, employeesToDelete);
                var response = new ApiResponse<bool>
                {
                    Success = true,
                    Status = 200,
                    Message = "Members deleted",
                    Data = true
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ApiResponse<int>
                {
                    Success = false,
                    Status = 500,
                    Message = ex.Message
                };
                return BadRequest(response);
            }

        }

    }
}
