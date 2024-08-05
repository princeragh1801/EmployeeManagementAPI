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
    public class PaginatedController : ControllerBase
    {
        private readonly IPaginatedService _paginatedService;

        public PaginatedController(IPaginatedService paginatedService)
        {
            _paginatedService = paginatedService;
        }

        [HttpPost("employees")]
        public async Task<ActionResult<ApiResponse<PaginatedItemsDto<List<EmployeeDto>>>>> GetEmployees(PaginatedDto paginatedDto)
        {
            try
            {
                var employees = await _paginatedService.GetEmployees(paginatedDto);

                var response = new ApiResponse<PaginatedItemsDto<List<EmployeeDto>>>
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
        
        [HttpPost("departments")]
        public async Task<ActionResult<ApiResponse<PaginatedItemsDto<List<DepartmentDto>>>>> GetDepartments(PaginatedDto paginatedDto)
        {
            try
            {
                var departments = await _paginatedService.GetDepartments(paginatedDto);

                var response = new ApiResponse<PaginatedItemsDto<List<DepartmentDto>>>
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
                return BadRequest(new ApiResponse<List<DepartmentDto>>
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
                var projects = await _paginatedService.GetProjects(paginatedDto);

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
                var tasks = await _paginatedService.GetTasks(paginatedDto);

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
