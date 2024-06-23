using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SolucionPeakHours.Interfaces;
using SolucionPeakHours.Repositories;
using SolucionPeakHours.Shared.Dashboard;

namespace SolucionPeakHours.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IAccountService _authService;

        public DashboardController(IDashboardRepository dashboardRepository, IAccountService authService)
        {
            _authService = authService;
            _dashboardRepository = dashboardRepository;
        }

        [HttpGet("getEmployeesWithDetails")]
        [Authorize]
        public async Task<ActionResult<EmployeeDashboard>> GetEmployeesWithDetails()
        {
            var response = await _dashboardRepository.GetEmployeesWithDetails();

            var userIdLogged = User
                .Claims
                .FirstOrDefault(x => x.Type == "Id")!
                .Value;

            var user = await _authService.GetUserById(userIdLogged);

            if (user.Role != "Administrator")
            {
                response = await _dashboardRepository.GetEmployeesWithDetailsByUserArea(user.FactoryStaffEntity!.Area!);
            }

            return Ok(response);
        }

        [HttpGet("getTotalHoursByArea")]
        [Authorize]
        public async Task<IActionResult> GetTotalHoursByArea(string area, int lastByDay)
        {
            var response = await _dashboardRepository.GetTotalHoursByArea(area, lastByDay);

            var userIdLogged = User
                .Claims
                .FirstOrDefault(x => x.Type == "Id")!
                .Value;

            var user = await _authService.GetUserById(userIdLogged);

            if(user.Role != "Administrator")
            {
                response = await _dashboardRepository.GetTotalHoursByUserArea(lastByDay, user.FactoryStaffEntity.Area);
            }

            return Ok(response);
        }

        [HttpGet("getTotalHoursByEmployee")]
        [Authorize]
        public async Task<IActionResult> GetTotalHoursByEmployee(int emplooyeeId, int lastByDay)
        {
            var response = await _dashboardRepository.GetTotalHoursByEmployee(emplooyeeId, lastByDay);

            var userIdLogged = User
                .Claims
                .FirstOrDefault(x => x.Type == "Id")!
                .Value;

            var user = await _authService.GetUserById(userIdLogged);

            if (user.Role != "Administrator")
            {
                response = await _dashboardRepository.GetTotalHoursByEmployeeByUserArea(emplooyeeId, lastByDay, user.FactoryStaffEntity.Area);
            }

            return Ok(response);
        }

        [HttpGet("getEmployeesWithMoreHours")]
        [Authorize]
        public async Task<IActionResult> GetEmployeesWithMoreHours()
        {
            var response = await _dashboardRepository.GetEmployeesWithMoreHours();

            var userIdLogged = User
                .Claims
                .FirstOrDefault(x => x.Type == "Id")!
                .Value;

            var user = await _authService.GetUserById(userIdLogged);

            if (user.Role != "Administrator")
            {
                response = await _dashboardRepository.GetEmployeesWithMoreHoursByUserArea(user.FactoryStaffEntity!.Area!);
            }

            return Ok(response);
        }
    }
}
