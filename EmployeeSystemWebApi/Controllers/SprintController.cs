namespace EmployeeSystemWebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SprintController : ControllerBase
    {
        private readonly ISprintService _sprintService;
        public SprintController(ISprintService sprintService)
        {
            _sprintService = sprintService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<SprintInfo>>>> GetAll()
        {
            try
            {
                var res = await _sprintService.GetAll();
                var response = new ApiResponse<List<SprintInfo>>();
                response.Message = "Sprints fetched";
                response.Data = res;
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<SprintInfo>>> GetById(int id)
        {
            try
            {
                var res = await _sprintService.GetById(id);
                var response = new ApiResponse<SprintInfo?>();
                response.Message = "Sprint fetched";
                response.Data = res;
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("project/{id}")]
        public async Task<ActionResult<ApiResponse<List<SprintInfo>>>> GetByProjectId(int id)
        {
            try
            {
                var res = await _sprintService.GetByProjectId(id);
                var response = new ApiResponse<List<SprintInfo>>();
                response.Message = "Sprint fetched";
                response.Data = res;
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpPost("{id=0}")]
        public async Task<ActionResult<ApiResponse<int>>> Upsert(int id, AddSprintDto addSprintDto)
        {
            try
            {
                var claims = HttpContext.User.Claims;
                var res = await _sprintService.Upsert(id, claims, addSprintDto);
                var response = new ApiResponse<int>();
                response.Message = "Sprint added";
                response.Data = res;
                if(res == 0)
                {
                    response.Status = 409;
                    response.Message = "Sprint with given name already exist";
                    return Conflict(response);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
        {
            try
            {
                var claims = HttpContext.User.Claims;
                var res = await _sprintService.DeleteById(id, claims);
                var response = new ApiResponse<bool>();
                if (!res)
                {
                    response.Message = "Sprint not found";
                    response.Status = 404;
                    return NotFound(response);
                }
                response.Message = "Sprint deleted";
                response.Data = res;
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
