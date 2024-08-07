﻿using EmployeeSystem.Contract.Dtos;
using EmployeeSystem.Contract.Interfaces;
using EmployeeSystem.Contract.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
                // fetching id from token
                var userId = Convert.ToInt32(HttpContext.User.Claims.First(e => e.Type == "Id").Value);
                var adminId = Convert.ToInt32(HttpContext.User.Claims.First(e => e.Type == "UserId").Value);
                var id = await _projectService.Add(userId, adminId, project);
                var response = new ApiResponse<int>
                {
                    Success = true,
                    Status = 200,
                    Message = "Project Details fetched",
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
    }
}
