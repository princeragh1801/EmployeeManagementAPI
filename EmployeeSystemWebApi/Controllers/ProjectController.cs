using EmployeeSystem.Contract.Dtos;
using EmployeeSystem.Contract.Dtos.Add;
using EmployeeSystem.Contract.Dtos.IdAndName;
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
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<ProjectDto>>>> GetAll()
        {
           try
           {
                var id = Convert.ToInt32(HttpContext.User.Claims.First(e=> e.Type == "UserId")?.Value);

                var projects = await _projectService.GetAll(id);
                var response = new ApiResponse<List<ProjectDto>>
                {
                    Success = true,
                    Status = 200,
                    Message = "Project Details fetched",
                    Data = projects
                };

                return Ok(response);
           }catch (Exception ex)
            {
                var response = new ApiResponse<List<ProjectDto>>
                {
                    Success = false,
                    Status = 500,
                    Message = ex.Message
                };
                return BadRequest(response);
            }

        }

        [HttpGet("employee/{employeeId}")]
        public async Task<ActionResult<ApiResponse<List<ProjectDto>>>> GetEmployeeProject(int employeeId)
        {
            try
            {
                
                var projects = await _projectService.GetProjectsByEmployee(employeeId);
                var response = new ApiResponse<List<ProjectDto>>
                {
                    Success = true,
                    Status = 200,
                    Message = "Project Details fetched",
                    Data = projects
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ApiResponse<List<ProjectDto>>
                {
                    Success = false,
                    Status = 500,
                    Message = ex.Message
                };
                return BadRequest(response);
            }

        }

        [HttpGet("StatusWise/{status}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult<ApiResponse<List<ProjectDto>>>> GetProjectStatusWise(ProjectStatus status)

        {
            try
            {

                var projects = await _projectService.GetProjectsStatusWise(status);
                var response = new ApiResponse<List<ProjectDto>>
                {
                    Success = true,
                    Status = 200,
                    Message = "Project Details fetched",
                    Data = projects
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ApiResponse<List<ProjectDto>>
                {
                    Success = false,
                    Status = 500,
                    Message = ex.Message
                };
                return BadRequest(response);
            }

        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ProjectDetailsDto>>> GetById(int id)
        {
            try
            {
                var project = await _projectService.GetById(id);
                var response = new ApiResponse<ProjectDetailsDto>
                {
                    Success = true,
                    Status = 200,
                    Message = "Project Details fetched",
                    Data = project
                };
                if(project == null)
                {
                    response.Message = "Project not found";
                    response.Status = 404;
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ApiResponse<ProjectDetailsDto>
                {
                    Success = false,
                    Status = 500,
                    Message = ex.Message
                };
                return BadRequest(response);
            }

        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult<ApiResponse<int>>> Add(AddProjectDto project)
        {
            try
            {
                var adminId = Convert.ToInt32(HttpContext.User.Claims.First(e => e.Type == "UserId").Value);
                var id = await _projectService.Add(adminId, project);
                var response = new ApiResponse<int>
                {
                    Success = true,
                    Status = 200,
                    Message = "Project Added",
                    Data = id
                };

                if(id == 0)
                {
                    response.Message = "Unauthorized request";
                }

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

        [HttpPost("addmembers{projectId}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult<ApiResponse<bool>>> AddMembers(int projectId, List<int> employeesToAdd)
        {
            try
            {
                var add = await _projectService.AddMembers(projectId, employeesToAdd);
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

        [HttpDelete("deletemembers{projectId}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteMembers(int projectId, List<int> employeesToDelete)
        {
            try
            {
                var add = await _projectService.DeleteMembers(projectId, employeesToDelete);
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


        [HttpPut("{id}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult<ApiResponse<int>>> Upate(int id, AddProjectDto project)
        {
            try
            {
                var adminId = Convert.ToInt32(HttpContext.User.Claims.First(e => e.Type == "UserId").Value);
                var projectId = await _projectService.Update(id, adminId, project);
                var response = new ApiResponse<int>
                {
                    Success = true,
                    Status = 200,
                    Message = "Project Updated",
                    Data = projectId
                };
                if(id == -1)
                {
                    response.Status = 404;
                    response.Message = "Project not found";
                    return NotFound(response);
                }
                if (id == 0)
                {
                    response.Message = "Unauthorized request";
                }

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


        [HttpDelete("{id}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
        {
            try
            {
                var deleted = await _projectService.DeleteById(id);
                var response = new ApiResponse<bool>
                {
                    Success = true,
                    Status = 200,
                    Message = "Project Details fetched",
                    Data = deleted
                };

                if(!deleted)
                {
                    response.Status = 404;
                    response.Message = "Project not found";
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ApiResponse<bool>
                {
                    Success = false,
                    Status = 500,
                    Message = ex.Message
                };
                return BadRequest(response);
            }

        }

        [HttpGet("projectEmployees{projectId}")]
        public async Task<ActionResult<ApiResponse<List<EmployeeIdAndName>>>> GetProjectEmployees(int projectId)
        {
            try
            {
                var projectEmployees = await _projectService.GetProjectEmployees(projectId);
                var response = new ApiResponse<List<EmployeeIdAndName>>
                {
                    Success = true,
                    Status = 200,
                    Message = "Project Details fetched",
                    Data = projectEmployees
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ApiResponse<List<EmployeeIdAndName>>
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
