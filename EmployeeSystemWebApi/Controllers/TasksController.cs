using EmployeeSystem.Contract.Dtos;
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
                var tasks = await _taskService.GetAllTasks();
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

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<TasksDto>>> GetTaskById(int id)
        {
            try
            {
                var task = await _taskService.GetById(id);
                var response = new ApiResponse<TasksDto>
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
                return BadRequest(new ApiResponse<TasksDto?>
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
                var userId = Convert.ToInt32(HttpContext.User.Claims.First(e => e.Type == "Id").Value);

                var id = await _taskService.Add(userId, taskDto);
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
                }else if(id == -1)
                {
                    response.Message = "Employee is not in the project so task can't assign";
                }else if(id == -2)
                {
                    response.Message = "Project not exist";
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
        public async Task<ActionResult<ApiResponse<bool>>> DeleteTask(int id)
        {
            try
            {
                var deleted = await _taskService.Delete(id);
                var response = new ApiResponse<bool>
                {
                    Success = true,
                    Status = 200,
                    Message = "Task Added",
                    Data = deleted
                };
                if (!deleted)
                {
                    response.Success = false;
                    response.Status = 404;
                    response.Message = "Task with given id not exist";
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
        public async Task<ActionResult<ApiResponse<TasksDto>>> UpdateTaskStatus(int id, TasksStatus status)
        {
            try
            {
                // fetching id from token
                var userId = Convert.ToInt32(HttpContext.User.Claims.First(e => e.Type == "Id").Value);

                var task = await _taskService.UpdateStatus(userId, id, status);
                var response = new ApiResponse<TasksDto>
                {
                    Success = true,
                    Status = 200,
                    Message = "Task status updated",
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
    }
}
