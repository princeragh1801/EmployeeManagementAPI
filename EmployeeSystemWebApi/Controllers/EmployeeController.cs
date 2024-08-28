using EmployeeSystem.Contract.Dtos;
using EmployeeSystem.Contract.Dtos.Add;
using EmployeeSystem.Contract.Dtos.Count;
using EmployeeSystem.Contract.Dtos.IdAndName;
using EmployeeSystem.Contract.Dtos.Info;
using EmployeeSystem.Contract.Dtos.Info.PaginationInfo;
using EmployeeSystem.Contract.Interfaces;
using EmployeeSystem.Contract.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static EmployeeSystem.Contract.Enums.Enums;

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

        [HttpGet("Count")]
        public async Task<ActionResult<ApiResponse<EmployeeCount>>> GetCount()
        {
            try
            {
                var count = await _employeeService.GetCounts();
                var response = new ApiResponse<EmployeeCount>();
                response.Data = count;
                response.Message = "Details fetched";
                return Ok(count);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("pagination")]
        public async Task<ActionResult<ApiResponse<PaginatedItemsDto<List<EmployeePaginationInfo>>>>> GetEmployees(PaginatedDto<Role?> paginatedDto)
        {
            try
            {
                var userId = Convert.ToInt32(HttpContext.User.Claims.First(e => e.Type == "UserId")?.Value);
                var employees = await _employeeService.Get(userId, paginatedDto);

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

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<EmployeeDto>?>>> GetEmployees()
        {
            try
            {
                var userId = Convert.ToInt32(HttpContext.User.Claims.First(e => e.Type == "UserId").Value);

                var employees = await _employeeService.GetAll(userId);

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

        [HttpGet("employeeIdAndName")]
        public async Task<ActionResult<ApiResponse<List<EmployeeIdAndName>?>>> GetEmployeeIdAndName()
        {
            try
            {
                Console.WriteLine("Employee Object : " + _employeeService.GetHashCode());
                var employees = await _employeeService.GetEmployeeIdAndName();

                var response = new ApiResponse<List<EmployeeIdAndName>>
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
                return BadRequest(new ApiResponse<List<EmployeeIdAndName>>
                {
                    Success = false,
                    Status = 500,
                    Message = ex.Message,
                    Data = null
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<EmployeeInfo?>>> GetEmployeeById(int id)
        {

            try
            {
                Console.WriteLine("Employee Object : " + _employeeService.GetHashCode());
                var employee = await _employeeService.GetById(id);

                var response = new ApiResponse<EmployeeInfo>
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
                var userId = Convert.ToInt32(HttpContext.User.Claims.First(e => e.Type == "UserId").Value);
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
                var userId = Convert.ToInt32(HttpContext.User.Claims.First(e => e.Type == "UserId").Value);

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
                    response.Message = "Employee with given details not exist or you're trying to delete a employee with the role of super-admin";
                    return NotFound(response);
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
        public async Task<ActionResult<ApiResponse<bool?>>> UpdateEmployee(int id, UpdateEmployeeDto employee)
        {
            try
            {
                // fetching id from token
                var userId = Convert.ToInt32(HttpContext.User.Claims.First(e => e.Type == "UserId").Value);

                var updatedEmployee = await _employeeService.Update(userId, id, employee);

                var response = new ApiResponse<bool?>
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
        public async Task<ActionResult<ApiResponse<List<EmployeeIdAndName>?>>> GetEmployeeDepartmentWise(int id)
        {
            try
            {
                var employees = await _employeeService.GetEmployeesWithDepartmentName(id);

                var response = new ApiResponse<List<EmployeeIdAndName>>
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
                return BadRequest(new ApiResponse<List<EmployeeIdAndName>>
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
