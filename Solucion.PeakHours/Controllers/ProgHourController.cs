using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SolucionPeakHours.Entities;
using SolucionPeakHours.Interfaces;
using SolucionPeakHours.Models;
using SolucionPeakHours.Shared.ProgHours;

namespace SolucionPeakHours.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProgHourController : ControllerBase
    {
        private readonly IBaseCrudRepository<ProgHourEntity> _employeeRepository;
        private readonly UserManager<UserIdentityEntity> _userManager;
        private readonly IProgHourRepository _progHourRepository;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public ProgHourController(IBaseCrudRepository<ProgHourEntity> employeeRepository,
                                      UserManager<UserIdentityEntity> userManager,
                                      IMapper mapper,
                                      IAccountService accountService,
                                      IProgHourRepository progHourRepository)
        {
            _userManager = userManager;
            _accountService = accountService;
            _employeeRepository = employeeRepository;
            _mapper = mapper;
            _progHourRepository = progHourRepository;
        }

        [HttpGet("Get")]
        [Authorize(Roles = "Administrator, GerenteArea, Supervisor")]
        public async Task<ActionResult<List<ProgHourDTO>>> Get()
        {
            var progHours = await _progHourRepository.GetAll();

            var userIdLogged = User
                .Claims
                .FirstOrDefault(x => x.Type == "Id")!
                .Value;

            var user = await _accountService.GetUserById(userIdLogged);

            if (user.Role != "Administrator")
            {
                progHours = await _progHourRepository.GetProgHourByUserArea(user
                    .FactoryStaffEntity!
                    .Area!);
            }

            var progHoursMapped = _mapper.Map<List<ProgHourDTO>>(progHours);

            return Ok(progHoursMapped);
        }

        [HttpGet("GetProgHoursByEmployeeId")]
        [Authorize(Roles = "Administrator, GerenteArea, Supervisor")]
        public async Task<ActionResult<ProgHourDTO>> GetProgHoursByEmployeeId(int employeeId)
        {
            var employees = await _progHourRepository.GetProgHoursByEmployeeId(employeeId);

            var employeesMapped = _mapper.Map<List<ProgHourDTO>>(employees);

            return Ok(employeesMapped);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Administrator, GerenteArea, Supervisor")]
        public async Task<ActionResult<ProgHourDTO>> Get(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            var employeeMapped = _mapper.Map<ProgHourDTO>(employee);
            return Ok(employeeMapped);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, GerenteArea, Supervisor")]
        public async Task<ActionResult<ProgHourDTO>> Post(ProgHourDTO progHourRequest)
        {
            var userIdLogged = User
                .Claims
                .FirstOrDefault(x => x.Type == "Id")!
                .Value;

            var progHour = _mapper.Map<ProgHourEntity>(progHourRequest);

            progHour.CreatedByUserId = userIdLogged;

            progHour.Id = null;

            await _employeeRepository.CreateAsync(progHour);

            var user = await _accountService.GetUserById(userIdLogged);

            user.AllowEmergencyTime = false;

            await _userManager.UpdateAsync(user);

            var createdEmployeeDto = _mapper.Map<ProgHourDTO>(progHour);

            return CreatedAtAction(nameof(Get), new { id = createdEmployeeDto.Id }, createdEmployeeDto);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator, GerenteArea, Supervisor")]
        public async Task<ActionResult> Put(int id, [FromBody] ProgHourDTO employeeDto)
        {
            var existingEmployee = await _employeeRepository.GetByIdAsync(id);

            if (existingEmployee == null)
            {
                return NotFound();
            }

            var employeeToUpdate = _mapper.Map(employeeDto, existingEmployee);
            var employeeUpdated = await _employeeRepository.UpdateAsync(employeeToUpdate);
            return Ok(employeeUpdated);
        }

        [HttpDelete("Delete")]
        [Authorize(Roles = "Administrator, GerenteArea, Supervisor")]
        public async Task<ActionResult> Delete(int IdPerson)
        {
            var existingEmployee = await _employeeRepository.GetByIdAsync(IdPerson);

            if (existingEmployee == null)
            {
                return NotFound();
            }

            await _employeeRepository.DeleteAsync(existingEmployee);
            return Ok();
        }
    }
}