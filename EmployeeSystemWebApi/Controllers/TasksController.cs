using AutoMapper;
using EmployeeSystem.Contract.Dtos;
using EmployeeSystem.Contract.Dtos.Add;
using EmployeeSystem.Contract.Dtos.Count;
using EmployeeSystem.Contract.Dtos.Info;
using EmployeeSystem.Contract.Dtos.Info.PaginationInfo;
using EmployeeSystem.Contract.Interfaces;
using EmployeeSystem.Contract.Models;
using EmployeeSystem.Contract.Response;
using EmployeeSystem.Provider;
using EmployeeSystem.Provider.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using static EmployeeSystem.Contract.Enums.Enums;

namespace EmployeeSystemWebApi.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ITaskService _taskService;
        private readonly ApplicationDbContext _context;
        private readonly ITaskLogService _taskLogService;
        public TasksController(ITaskService taskService, ApplicationDbContext applicationDbContext, ITaskLogService taskLogService, IMapper mapper)
        {
            _taskService = taskService;
            _context = applicationDbContext;
            _taskLogService = taskLogService;   
            _mapper = mapper;
        }

        [HttpGet("Count")]
        public async Task<ActionResult<ApiResponse<TaskCount>>> GetCount()
        {
            try
            {
                var count = await _taskService.GetCount();
                var response = new ApiResponse<TaskCount>();
                response.Data = count;
                response.Message = "Details fetched";
                return Ok(count);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("pagination/{projectId}")]
        public async Task<ActionResult<ApiResponse<PaginatedItemsDto<List<TasksDto>>>>> GetProjectTasks(int projectId, ProjectTasksDto paginatedDto)
        {
            try
            {
                var userId = Convert.ToInt32(HttpContext.User.Claims.First(e => e.Type == "UserId")?.Value);
                var tasks = await _taskService.Get(userId, projectId, paginatedDto);

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
        public async Task<ActionResult<ApiResponse<bool?>>> UpdateTask(int id, UpdateTaskDto taskDto)
        {
            try
            {
                // fetching id from token
                var userId = Convert.ToInt32(HttpContext.User.Claims.First(e => e.Type == "UserId").Value);

                var task = await _taskService.Update(userId, id, taskDto);
                var response = new ApiResponse<bool?>
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
                return BadRequest(new ApiResponse<bool?>
                {
                    Success = false,
                    Status = 500,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("task{parentId}/children")]
        public async Task<ActionResult<ApiResponse<List<TasksDto>>>> GetChildren(int parentId)
        {
            try
            {
                var response = new ApiResponse<List<TasksDto>>();

                response.Data = await _taskService.GetChildren(parentId);
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

        [HttpPost("link-task-sprint/{taskId}")]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateTaskSprint([FromBody] int sprintId, int taskId)
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
        public async Task<ActionResult<ApiResponse<bool>>> UpdateTaskStatus(int taskId, [FromBody] TasksStatus status)
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
        public async Task<ActionResult<ApiResponse<bool>>> UpdateParent(int taskId, [FromBody] int parentId)
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

        [HttpPatch("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> PatchUpdate(int id, [FromBody] JsonPatchDocument<UpdateTaskDto> patchDoc)
        {
            try
            {
                var name = HttpContext.User.Claims.First(e => e.Type == "Name").Value;
                var response = new ApiResponse<bool>();
                var userId = Convert.ToInt32(HttpContext.User.Claims.First(e => e.Type == "UserId").Value);
                if (patchDoc == null)
                {
                    return BadRequest();
                }
                var task = await _taskService.GetById(id);
                if (task == null)
                {
                    response.Message = "task not found";
                    response.Status = 200;
                    return NotFound(response);
                }

                var originalValues = _context.Entry(task).CurrentValues.Clone();

                var taskToPatch = _mapper.Map<UpdateTaskDto>(task);

                patchDoc.ApplyTo(taskToPatch, ModelState);

                if (!ModelState.IsValid)
                {
                    return ValidationProblem(ModelState);
                }

                _mapper.Map(taskToPatch, task);

                var changedEntries = new List<string>();

                foreach (var property in _context.Entry(task).Properties)
                {
                    var originalValue = originalValues[property.Metadata.Name]?.ToString();
                    var currentValue = property.CurrentValue?.ToString();

                    if (originalValue != currentValue)
                    {
                        changedEntries.Add($"{property.Metadata.Name}: '{originalValue}' -> '{currentValue}'");
                    }
                }
                var count = changedEntries.Count();
                Console.WriteLine("Changed Entries : " + count);
                if (changedEntries.Any())
                {
                    var log = new AddTaskLogDto
                    {
                        TaskId = id,
                        Message = $"Task updated by {name} on {DateTime.Now}. Changes: {string.Join(", ", changedEntries)}"
                    };
                    Console.WriteLine(log);
                    await _taskLogService.Add(log);
                }

                await _taskService.UpdateTask(task);
                response.Message = "Updated the task details";
                response.Data = true;
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
