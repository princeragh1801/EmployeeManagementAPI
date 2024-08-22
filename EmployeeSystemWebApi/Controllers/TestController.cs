using EmployeeSystem.Contract.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeSystemWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private int id;
        private readonly ITestService _testService;
        public TestController(ITestService testService)
        {
            _testService = testService;
            
        }

        [HttpGet]
        public ActionResult Get()
        {
            try
            {
                var res = _testService.Get();
                return Ok(res);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
