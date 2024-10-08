﻿namespace EmployeeSystemWebApi.Controllers
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

        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpGet("Count")]
        public async Task<ActionResult<ApiResponse<ProjectCount>>> GetCount()
        {
            try
            {
                var count = await _projectService.GetCounts();
                var response = new ApiResponse<ProjectCount>();
                response.Data = count;
                response.Message = "Details fetched";
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("pagination")]
        public async Task<ActionResult<ApiResponse<PaginatedItemsDto<List<ProjectDto>>>>> GetProjects(PaginatedDto<ProjectStatus?> paginatedDto)
        {
            try
            {
                var claims = HttpContext.User.Claims;
                var projects = await _projectService.Get(claims, paginatedDto);

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


        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<ProjectDto>>>> GetAll()
        {
            try
            {
                var claims = HttpContext.User.Claims;
                var projects = await _projectService.GetAll(claims);
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

        [HttpGet("employee/{employeeId}")]
        public async Task<ActionResult<ApiResponse<List<EmployeeProjectInfo>>>> GetEmployeeProject(int employeeId)
        {
            try
            {
                var claims = HttpContext.User.Claims;
                var projects = await _projectService.GetProjectsByEmployee(claims, employeeId);
                var response = new ApiResponse<List<EmployeeProjectInfo>>
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
                var response = new ApiResponse<List<EmployeeProjectInfo>>
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
                var claims = HttpContext.User.Claims;
                var project = await _projectService.GetById(claims, id);
                var response = new ApiResponse<ProjectDetailsDto>
                {
                    Success = true,
                    Status = 200,
                    Message = "Project Details fetched",
                    Data = project
                };
                if (project == null)
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
        [Authorize(Roles = "SuperAdmin, Admin")]
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

        [HttpPut("{id}")]
        [Authorize(Roles = "SuperAdmin, Admin")]
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
                if (id == -1)
                {
                    response.Status = 404;
                    response.Message = "Project not found";
                    return NotFound(response);
                }
                if (id == 0 || id == -2)
                {
                    response.Message = "Unauthorized request";
                    return Forbid();
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
        [Authorize(Roles = "SuperAdmin, Admin")]
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

                if (!deleted)
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


    }
}
