using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SolucionPeakHours.Interfaces;

namespace SolucionPeakHours.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BackgroundTaskController : ControllerBase
    {
        private readonly IBackgroundTaskService _backgroundTaskService;

        public BackgroundTaskController(IBackgroundTaskService backgroundTaskService)
        {
            _backgroundTaskService = backgroundTaskService;
        }

        [HttpGet("ExpireEntitiesTaskJob")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> ExpireEntitiesTaskJob()
        {
            await _backgroundTaskService.ExpireEntitiesTaskJob();
            return Ok();
        }

        [HttpGet("RestartTotalHourPerMonthJob")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> RestartTotalHourPerMonthJob()
        {
            await _backgroundTaskService.RestartTotalHourPerMonthJob();
            return Ok();
        }
    }
}
