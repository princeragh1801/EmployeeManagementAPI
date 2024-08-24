using EmployeeSystem.Contract.Dtos.Info;
using EmployeeSystem.Contract.Interfaces;
using EmployeeSystem.Contract.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeSystemWebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TaskLogController : ControllerBase
    {
        private readonly ITaskLogService _taskLogService;

        public TaskLogController(ITaskLogService taskLogService)
        {
            _taskLogService = taskLogService;
        }

        [HttpGet("{taskId}")]
        public async Task<ActionResult<ApiResponse<List<LogDto>>>> GetLogs(int taskId)
        {
            try
            {
                var logs = await _taskLogService.GetLogs(taskId);
                var response = new ApiResponse<List<LogDto>>();
                response.Data = logs;
                response.Message = "Task logs fetched";
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
