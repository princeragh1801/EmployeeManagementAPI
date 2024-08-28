using EmployeeSystem.Contract.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeSystemWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IEmailService _emailService;
        public MessageController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost]
        public IActionResult Message(string to, string msg)
        {
            try
            {
                _emailService.SendWhatsAppMessage(to, msg);
                return Ok("Message sent");
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
