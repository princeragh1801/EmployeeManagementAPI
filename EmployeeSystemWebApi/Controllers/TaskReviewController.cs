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
    public class TaskReviewController : ControllerBase
    {
        private readonly ITaskReviewService _taskReviewService;

        public TaskReviewController(ITaskReviewService taskReviewService)
        {
            _taskReviewService = taskReviewService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<List<TaskReviewDto>?>>> GetTaskReviews(int id)
        {
            try
            {
                var reviews = await _taskReviewService.Get(id);
                var response = new ApiResponse<List<TaskReviewDto>?>
                {
                    Success = true,
                    Status = 200,
                    Message = "Reviews fetched",
                    Data = reviews
                };
                return Ok(response);
            }catch (Exception ex)
            {
                return BadRequest(new ApiResponse<List<TaskReviewDto>?>{
                    Success = false,
                    Status = 500,
                    Message = ex.Message
                });
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<int>>> AddNewReview(AddTaskReviewDto taskReviewDto)
        {
            try
            {
                // fetching id from token
                var userId = Convert.ToInt32(HttpContext.User.Claims.First(e => e.Type == "Id").Value);
                var adminId = Convert.ToInt32(HttpContext.User.Claims.First(e => e.Type == "UserId").Value);

                var id =  await _taskReviewService.Add(userId, adminId, taskReviewDto);
                var response = new ApiResponse<int>
                {
                    Success = true,
                    Status = 200,
                    Message = "Review added",
                    Data = id
                };
                
                return Ok(response);
            }catch (Exception ex)
            {
                return BadRequest(new ApiResponse<int>
                {
                    Success = false,
                    Status = 500,
                    Message = ex.Message
                });
            }
        }
    }
}
