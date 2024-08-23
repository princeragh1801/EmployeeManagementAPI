﻿using EmployeeSystem.Contract.Dtos;
using EmployeeSystem.Contract.Dtos.Add;
using EmployeeSystem.Contract.Dtos.Info;
using EmployeeSystem.Contract.Interfaces;
using EmployeeSystem.Contract.Models;
using EmployeeSystem.Contract.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static EmployeeSystem.Contract.Enums.Enums;

namespace EmployeeSystemWebApi.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<TasksDto>>>> GetAllTasks()
        {
            try
            {
                // fetching id from token
                var userId = Convert.ToInt32(HttpContext.User.Claims.First(e => e.Type == "UserId").Value);
                var tasks = await _taskService.GetAllTasks(userId);
                var response = new ApiResponse<List<TasksDto>>
                {
                    Success = true,
                    Status = 200,
                    Message = "Task fetched",
                    Data = tasks
                };
                return Ok(response);
            }catch (Exception ex)
            {
                return BadRequest(new ApiResponse<List<TasksDto>?>
                {
                    Success = false,
                    Status = 500,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("employee/{employeeId}")]
        public async Task<ActionResult<ApiResponse<List<TasksDto>>>> GetEmployeeTasks(int employeeId)
        {
            try
            {
                
                var tasks = await _taskService.GetEmployeeTask(employeeId);
                var response = new ApiResponse<List<TasksDto>>
                {
                    Success = true,
                    Status = 200,
                    Message = "Task fetched",
                    Data = tasks
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<List<TasksDto>?>
                {
                    Success = false,
                    Status = 500,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("sprint/{sprintId}")]
        public async Task<ActionResult<ApiResponse<List<TasksDto>>>> GetSprintTasks(int sprintId)
        {
            try
            {

                var tasks = await _taskService.GetSprintTask(sprintId);
                var response = new ApiResponse<List<TasksDto>>
                {
                    Success = true,
                    Status = 200,
                    Message = "Task fetched",
                    Data = tasks
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<List<TasksDto>?>
                {
                    Success = false,
                    Status = 500,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<TaskInfo>>> GetTaskById(int id)
        {
            try
            {
                // fetching id from token
                var userId = Convert.ToInt32(HttpContext.User.Claims.First(e => e.Type == "UserId").Value);
                var task = await _taskService.GetById(userId, id);
                var response = new ApiResponse<TaskInfo>
                {
                    Success = true,
                    Status = 200,
                    Message = "Task fetched",
                    Data = task
                };
                if(task == null)
                {
                    response.Message = "Task with given id not found";
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<TaskInfo?>
                {
                    Success = false,
                    Status = 500,
                    Message = ex.Message
                });
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<int>>> AddNewTask(AddTaskDto taskDto)
        {
            try
            {
                // fetching id from token
                var adminId = Convert.ToInt32(HttpContext.User.Claims.First(e => e.Type == "UserId").Value);

                var id = await _taskService.Add(adminId, taskDto);
                var response = new ApiResponse<int>
                {
                    Success = true,
                    Status = 200,
                    Message = "Task Added",
                    Data = id
                };
                if (id == 0)
                {
                    response.Message = "Unauthorized request, The assigner doesn't has the authority to assign the task";
                    response.Status = 401;
                    return Unauthorized(response);
                }else if(id == -1)
                {
                    response.Message = "Employee is not in the project so task can't assign";
                    response.Status = 401;
                    return Unauthorized(response);
                }else if(id == -2)
                {
                    response.Message = "Project not exist";
                    response.Status = 404;
                    return NotFound(response);
                }else if(id == -3)
                {
                    response.Message = "User with assigned to id is not exist";
                    response.Status = 404;
                    return NotFound(response);
                }else if(id == -4)
                {
                    response.Message = "Invalid parent";
                    response.Status = 422;
                    return UnprocessableEntity(response);
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

        [HttpPost("addMany")]
        public async Task<ActionResult<ApiResponse<bool>>> AddList(List<AddTaskDto> taskDto)
        {
            try
            {
                // fetching id from token
                var adminId = Convert.ToInt32(HttpContext.User.Claims.First(e => e.Type == "UserId").Value);

                var added = await _taskService.AddMany(adminId, taskDto);
                var response = new ApiResponse<bool>
                {
                    Success = true,
                    Status = 200,
                    Message = "Task Added",
                    Data = added
                };
                if (!added)
                {
                    response.Message = "Unauthorized request, The assigner doesn't has the authority to assign the task";
                    response.Status = 401;
                    return Unauthorized(response);
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


        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteTask(int id)
        {
            try
            {
                // fetching id from token
                var userId = Convert.ToInt32(HttpContext.User.Claims.First(e => e.Type == "UserId").Value);

                var deleted = await _taskService.Delete(userId, id);
                var response = new ApiResponse<bool>
                {
                    Success = true,
                    Status = 200,
                    Message = "Task Deleted",
                    Data = true
                   
                };
                if(deleted == null)
                {
                    response.Message = "Unauthorized user";
                    response.Data = false;
                    return Unauthorized(response);
                }
                else if (deleted == false)
                {
                    response.Success = false;
                    response.Status = 404;
                    response.Data = false;
                    response.Message = "Task with given id not exist";
                    return NotFound(response);
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

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<TasksDto>>> UpdateTask(int id, UpdateTaskDto taskDto)
        {
            try
            {
                // fetching id from token
                var userId = Convert.ToInt32(HttpContext.User.Claims.First(e => e.Type == "UserId").Value);

                var task = await _taskService.Update(userId, id, taskDto);
                var response = new ApiResponse<TasksDto>
                {
                    Success = true,
                    Status = 200,
                    Message = "Task details updated",
                    Data = task
                };
                if (task == null)
                {
                    response.Message = "Task not found";
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<TasksDto?>
                {
                    Success = false,
                    Status = 500,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("epics")]
        public async Task<ActionResult<ApiResponse<List<EpicTaskDto>>>> GetEpicTasks()
        {
            try
            {
                var response = new ApiResponse<List<EpicTaskDto>>();
                
                response.Data = await _taskService.GetEpics();
                response.Message = "Epics fetched";
                return Ok(response);
            }catch (Exception ex)
            {
                return BadRequest(new ApiResponse<List<EpicTaskDto>?>
                {
                    Success = false,
                    Status = 500,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("task{parentId}/children")]
        public async Task<ActionResult<ApiResponse<List<TasksDto>>>> GetEpicTasks(int parentId)
        {
            try
            {
                var response = new ApiResponse<List<TasksDto>>();

                response.Data = await _taskService.GetChilds(parentId);
                response.Message = "Childs fetched";
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<List<TasksDto>?>
                {
                    Success = false,
                    Status = 500,
                    Message = ex.Message
                });
            }
        }

        [HttpPost("update-sprint/{sprintId}")]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateTaskSprint(int sprintId, int taskId)
        {
            try
            {
                // fetching id from token
                var userId = Convert.ToInt32(HttpContext.User.Claims.First(e => e.Type == "UserId").Value);

                var updated = await _taskService.UpdateTaskSprint(sprintId, taskId);
                var response = new ApiResponse<bool>
                {
                    Success = true,
                    Status = 200,
                    Message = "Task added to sprint",
                    Data = true

                };
                if (!updated)
                {
                    response.Message = "Sprint or task not found or not belongs to the same project";
                    response.Data = false;
                    return NotFound(response);
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

        [HttpPut("update-status/{taskId}")]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateTaskStatus(int taskId, TasksStatus status)
        {
            try
            {
                var updateStatus = await _taskService.UpdateTaskStatus(taskId, status);
                var response = new ApiResponse<bool>();
                response.Data = updateStatus;
                if (!updateStatus)
                {
                    response.Message = "Task not found";
                    response.Status = 404;

                    return NotFound();
                }
                response.Message = "Status updated";
                return Ok(response);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("link-parent/{taskId}")]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateParent(int taskId, int parentId)
        {
            try
            {
                var updateParent = await _taskService.UpdateTaskParent(taskId, parentId);
                var response = new ApiResponse<bool>();
                response.Data = updateParent;
                if (!updateParent)
                {
                    response.Message = "Not valid assign";
                    response.Status = 404;

                    return NotFound(response);
                }
                response.Message = "Parent updated";
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("logs/{taskId}")]
        public async Task<ActionResult<ApiResponse<List<LogDto>>>> GetLogs(int taskId)
        {
            try
            {
                var logs = await _taskService.GetLogs(taskId);
                var response = new ApiResponse<List<LogDto>>();
                response.Data = logs;
                response.Message = "Task logs fetched";
                return Ok(response);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
