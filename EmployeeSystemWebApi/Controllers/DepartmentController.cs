using EmployeeSystem.Contract.Dtos;
using EmployeeSystem.Contract.Dtos.Add;
using EmployeeSystem.Contract.Interfaces;
using EmployeeSystem.Contract.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeSystemWebApi.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    [Route("[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }


        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<DepartmentDto>>>> GetAllDepartments()
        {
            try
            {
                var response = await _departmentService.GetAll();
                
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

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<DepartmentDto?>>> GetDepartmentById(int id)
        {
            try
            {
                var response = await _departmentService.GetById(id);
                
                if(response.Status == 404)
                {
                    return NotFound(response);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<DepartmentDto>
                {
                    Success = false,
                    Status = 500,
                    Message = ex.Message,
                    Data = null
                });
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<int>>> AddNewDepartment(AddDepartmentDto departmentDto)
        {
            try
            {
                // fetching id from token
                int employeeId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(e => e.Type == "UserId")?.Value);
                var response = await _departmentService.Add(employeeId, departmentDto);

                if (response.Status == 409)
                {
                    return NotFound(response);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<int>
                {
                    Success = false,
                    Status = 500,
                    Message = ex.Message
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteDepartment(int id)
        {
            try
            {
                var response = await _departmentService.DeleteById(id);
                
                if (response.Status == 404)
                {
                    return NotFound();
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<bool>
                {
                    Success = false,
                    Status = 500,
                    Message = ex.Message
                });
            }
        }
    }
}
