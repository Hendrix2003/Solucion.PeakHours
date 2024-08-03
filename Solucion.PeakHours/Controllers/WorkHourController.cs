using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SolucionPeakHours.Interfaces;
using SolucionPeakHours.Models;
using SolucionPeakHours.Shared.CompletedHours;

namespace SolucionPeakHours.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkHourController : ControllerBase
    {
        private readonly IBaseCrudRepository<FactoryStaffEntity> _factoryRepository;
        private readonly IWorkHourRepository _workHourRepository;
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;

        public WorkHourController(IBaseCrudRepository<FactoryStaffEntity> factoryRepository,
                                  IMapper mapper,
                                  IAccountService accountService,
                                  IWorkHourRepository workHourRepository)
        {
            _accountService = accountService;
            _workHourRepository = workHourRepository;
            _mapper = mapper;
            _factoryRepository = factoryRepository;
        }

        [HttpGet("GetAll")]
        [Authorize("Administrator, GerenteArea, Supervisor")]
        public async Task<ActionResult<List<WorkHourDTO>>> GetAll()
        {
            var workHours = await _workHourRepository.GetAll();

            var workHoursMapped = _mapper.Map<List<WorkHourDTO>>(workHours);

            return Ok(workHoursMapped);
        }

        [HttpGet("GetWorkHoursByUserArea")]
        [Authorize(Roles = "Administrator, GerenteArea, Supervisor")]
        public async Task<ActionResult<List<WorkHourDTO>>> Get()
        {
            var workHours = await _workHourRepository.GetAll();

            var userIdLogged = User
                .Claims
                .FirstOrDefault(x => x.Type == "Id")!
                .Value;

            var user = await _accountService.GetUserById(userIdLogged);

            if (user.Role != "Administrator")
            {
                workHours = await _workHourRepository.GetWorkHoursByUserArea(user
                .FactoryStaffEntity!
                .Area!);
            }

            var workHoursMapped = _mapper.Map<List<WorkHourDTO>>(workHours);

            return Ok(workHoursMapped);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Administrator, GerenteArea, Supervisor")]
        public async Task<ActionResult<WorkHourDTO>> Get(int id)
        {
            var employee = await _workHourRepository.GetByIdAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            var employeeMapped = _mapper.Map<WorkHourDTO>(employee);
            return Ok(employeeMapped);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, GerenteArea, Supervisor")]
        public async Task<ActionResult<WorkHourDTO>> Post(WorkHourDTO workHourRequest)
        {
            var userIdLogged = User
                .Claims
                .FirstOrDefault(x => x.Type == "Id")!
                .Value;

            var workHourToCreate = _mapper.Map<WorkHourEntity>(workHourRequest);

            workHourToCreate.Id = null;
            workHourToCreate.CreatedByUserId = userIdLogged;

            var workHourCreatedResponse = await _workHourRepository.CreateAsync(workHourToCreate);

            var workHourCreated = await _workHourRepository.GetByIdAsync((int)workHourCreatedResponse.Id!);

            var factoryStaff = await _factoryRepository.GetByIdAsync((int)workHourCreated
                .ProgHourEntity!
                .FactoryStaffEntityId!);

            var factoryTotalHoursPerMonth = factoryStaff!.TotalHoursPerMonth == null ? 0 : factoryStaff!.TotalHoursPerMonth;

            var totalHours = factoryTotalHoursPerMonth + workHourCreatedResponse.HoursWorked;

            factoryStaff.TotalHoursPerMonth = totalHours;

            await _factoryRepository.UpdateAsync(factoryStaff);

            var response = _mapper.Map<WorkHourDTO>(workHourToCreate);

            return Ok(response);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator, GerenteArea, Supervisor")]
        public async Task<ActionResult> Put(int id, [FromBody] WorkHourDTO employeeDto)
        {
            var existingEmployee = await _workHourRepository.GetByIdAsync(id);

            if (existingEmployee == null)
            {
                return NotFound();
            }

            var employeeToUpdate = _mapper.Map(employeeDto, existingEmployee);
            var employeeUpdated = await _workHourRepository.UpdateAsync(employeeToUpdate);
            return Ok(employeeUpdated);
        }

        [HttpDelete("Delete")]
        [Authorize(Roles = "Administrator, GerenteArea, Supervisor")]
        public async Task<ActionResult> Delete(int IdPerson)
        {
            var existingEmployee = await _workHourRepository.GetByIdAsync(IdPerson);

            if (existingEmployee == null)
            {
                return NotFound();
            }

            await _workHourRepository.DeleteAsync(existingEmployee);
            return Ok();
        }

        [HttpPost("ApproveByAreaManager")]
        [Authorize(Roles = "Administrator, GerenteArea, Supervisor")]
        public async Task<ActionResult<ApproveResponse>> ApproveByAreaManager(int workHourId)
        {
            var response = await _workHourRepository.ApproveByAreaManager(workHourId);

            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost("ApproveByFactoryManager")]
        [Authorize(Roles = "Administrator, GerenteArea, Supervisor")]
        public async Task<ActionResult<ApproveResponse>> ApproveByFactoryManager(int workHourId)
        {
            var response = await _workHourRepository.ApproveByFactoryManager(workHourId);

            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}