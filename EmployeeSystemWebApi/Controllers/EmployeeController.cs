
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
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult<ApiResponse<List<EmployeeDto>?>>> GetEmployees()
        {
            try
            {
                var employees = await _employeeService.GetAll();

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
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<EmployeeDto?>>> GetEmployeeById(int id)
        {

            try
            {
                var employee = await _employeeService.GetById(id);

                var response = new ApiResponse<EmployeeDto>
                {
                    Success = true,
                    Status = 200,
                    Message = "Employee details fetched",
                    Data = employee
                };
                if (employee == null)
                {
                    response.Message = "Employee not found";
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<EmployeeDto>
                {
                    Success = false,
                    Status = 500,
                    Message = ex.Message,
                    Data = null
                });
            }
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult<ApiResponse<int>>> AddEmployee(AddEmployeeDto employee)
        {
            try
            {
                // fetching id from token
                var userId = Convert.ToInt32(HttpContext.User.Claims.First(e => e.Type == "Id").Value);
                Console.WriteLine("UserId : " +  userId);
                var id = await _employeeService.Add(userId, employee);

                var response = new ApiResponse<int>
                {
                    Success = true,
                    Status = 200,
                    Message = "Employee Added successfully",
                    Data = id
                };
                if(id == 0)
                {
                    response.Message = "Manager not exist";
                }else if(id == -1)
                {
                    response.Message = "Role is not assigned properly";
                }else if(id == -2)
                {
                    response.Message = "Given username is already exist";
                    return Conflict(response);
                }
                return Ok(response);
            }catch (Exception ex)
            {
                return BadRequest(new ApiResponse<bool>
                {
                    Success = false,
                    Status = 500,
                    Message = ex.Message,
                });
            }
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost("addMany")]
        public async Task<ActionResult<ApiResponse<int>>> AddEmployees(List<AddEmployeeDto> employees)
        {
            try
            {
                // fetching id from token
                var userId = Convert.ToInt32(HttpContext.User.Claims.First(e => e.Type == "Id").Value);

                var added = await _employeeService.AddList(userId, employees);
                var response = new ApiResponse<int>
                {
                    Success = true,
                    Status = 200,
                    Message = "Employees Added successfully",
                    Data = added
                };
                if(added == -1)
                {
                    response.Message = "Role is not assigned properly";
                }
                else if(added == -2)
                {
                    response.Message = "Manager is not assigned properly";
                }
                else if (added == -3)
                {
                    response.Message = "Given username is already exist";
                    return Conflict(response);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<int>
                {
                    Success = false,
                    Status = 500,
                    Message = ex.Message,
                });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteEmployeeById(int id)
        {
            try
            {
                var deleted = await _employeeService.DeleteById(id);
                var response = new ApiResponse<bool>
                {
                    Success = true,
                    Status = 200,
                    Message = "Employee deleted",
                    Data = deleted
                };
                if (!deleted)
                {
                    response.Message = "Employee with given details no exist";
                }
                return Ok(response);
            }
            catch(Exception ex)
            {
                return BadRequest(new ApiResponse<bool>
                {
                    Success = false,
                    Status = 500,
                    Message = ex.Message,
                });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult<ApiResponse<EmployeeDto?>>> UpdateEmployee(int id, AddEmployeeDto employee)
        {
            try
            {
                // fetching id from token
                var userId = Convert.ToInt32(HttpContext.User.Claims.First(e => e.Type == "Id").Value);

                var updatedEmployee = await _employeeService.Update(userId, id, employee);

                var response = new ApiResponse<EmployeeDto>
                {
                    Success = true,
                    Status = 200,
                    Message = "Employee details updated",
                    Data = updatedEmployee
                };
                if (updatedEmployee == null)
                {
                    response.Message = "Proper details not supplied";
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<EmployeeDto>
                {
                    Success = false,
                    Status = 500,
                    Message = ex.Message,
                    Data = null
                });
            }
        }

        
        [HttpGet("managers")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult<ApiResponse<List<EmployeeDto>?>>> GetManagers()
        {
            try
            {
                var managers = await _employeeService.GetManagers();
                var response = new ApiResponse<List<EmployeeDto>>
                {
                    Success = true,
                    Status = 200,
                    Message = "Employees details fetched",
                    Data = managers
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
        }

        [HttpGet("department/{id}")]
        public async Task<ActionResult<ApiResponse<List<EmployeeDto>?>>> GetEmployeeDepartmentWise(int id)
        {
            try
            {
                var employees = await _employeeService.GetEmloyeesWithDepartmentName(id);

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
        }

    }
}
