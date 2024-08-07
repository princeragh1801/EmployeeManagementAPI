using EmployeeSystem.Contract.Dtos;
using EmployeeSystem.Contract.Interfaces;
using EmployeeSystem.Contract.Response;
using EmployeeSystem.Provider;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeSystemWebApi.Controllers
{
    [Route("Users")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }


        /*[Authorize(Roles = "SuperAdmin")]
        [HttpPost("/Register")]
        public async Task<ActionResult<ApiResponse<User>>> Register(RegisterDto userDto)
        {
            try
            {
                var response = new ApiResponse<User>();

                var userId = Convert.ToInt32(HttpContext.User.Claims.First(e => e.Type == "Id").Value);
                var user = await _authService.Register(userId, userDto);

                if (user == null)
                {
                    response.Message = "User with given username already found";
                    response.Status = 409;
                    response.Data = user;
                    return Conflict(response);
                }

                response.Message = "User Added";
                response.Data = user;
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ApiResponse<User>();
                response.Message = ex.Message;
                response.Status = 500;
                response.Success = false;

                return BadRequest(response);
            }
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost("/RegisterMany")]
        public async Task<ActionResult<ApiResponse<bool>>> RegisterMany(List<RegisterDto> registerDtos)
        {
            try
            {
                var response = new ApiResponse<bool>();

                var userId = Convert.ToInt32(HttpContext.User.Claims.First(e => e.Type == "Id").Value);
                var added = await _authService.RegisterMany(userId, registerDtos);

                if (!added)
                {
                    response.Message = "User with given username already found";
                    response.Status = 409;
                    response.Data = false;
                    return Conflict(response);
                }

                response.Message = "User Added";
                response.Data = added;
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ApiResponse<bool>();
                response.Message = ex.Message;
                response.Status = 500;
                response.Success = false;

                return BadRequest(response);
            }
        }
*/


        [HttpPost("/Login")]
        public async Task<ActionResult<ApiResponse<LoginUserDto>>> Login(RegisterDto userDto)
        {
            try
            {
                var response = new ApiResponse<LoginUserDto>();
                string userName = userDto.Username;
                string password = userDto.Password;

                var user = await _authService.Login(userDto);

                if (user == null)
                {
                    response.Message = "User with given username or password not found";
                    response.Status = 404;
                    return NotFound(response);
                }
                
                response.Message = "User fetched";
                response.Data = user;
                
                return Ok(response);
            }catch(Exception ex)
            {
                var response = new ApiResponse<LoginUserDto>();
                response.Message = ex.Message;
                response.Status = 500;
                response.Success = false;

                return BadRequest(response);
            }
        }


    }
}
