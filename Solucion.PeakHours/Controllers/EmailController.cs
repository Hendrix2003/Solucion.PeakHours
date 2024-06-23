using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolucionPeakHours.Interfaces;
using SolucionPeakHours.Shared.UserAccount;

namespace SolucionPeakHours.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;

        }

        [HttpPost]
        [Authorize(Roles = "Administrator, GerenteArea, Supervisor")]

        public IActionResult SendEmail(EmailDTO request)
        {
            _emailService.SendEmail(request);
            return Ok();
        }
    }
}
