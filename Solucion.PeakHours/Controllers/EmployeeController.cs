using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SolucionPeakHours.Models;
using SolucionPeakHours.Interfaces;
using Microsoft.AspNetCore.Authorization;
using SolucionPeakHours.Persistence;
using SolucionPeakHours.Shared.FactoryStaff;

namespace SolucionPeakHours.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IBaseCrudRepository<FactoryStaffEntity> _employeeRepository;
        private readonly IMapper _mapper;
        private readonly CndDbContext _context;
        public EmployeeController(IBaseCrudRepository<FactoryStaffEntity> employeeRepository, 
                                  CndDbContext context,
                                  IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet("GetAreas")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<string>> GetAreas()
        {
            var query = await _employeeRepository.Query();

            var areas = query.Select(x => x.Area).Distinct();

            return Ok(areas);
        }



        [HttpGet("Get")]
        [Authorize(Roles = "Administrator, GerenteArea, Supervisor")]
        public async Task<ActionResult<FactoryStaffDTO>> Get()
        {
            var employees = await _employeeRepository.GetAll();

            var employeesMapped = _mapper.Map<List<FactoryStaffDTO>>(employees);

            return Ok(employeesMapped);
        }

        [HttpGet("GetEmployeesByArea")]
        [Authorize(Roles = "Administrator, GerenteArea, Supervisor ")]
        public async Task<ActionResult<FactoryStaffDTO>> GetEmployeesByArea(string area)
        {
            var query = await _employeeRepository.Query();

            var employees = query.Where(x => x.Area == area);

            var employeesMapped = _mapper.Map<List<FactoryStaffDTO>>(employees);

            return Ok(employeesMapped);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Administrator, Supervisor, GerenteArea")]
        public async Task<ActionResult<FactoryStaffDTO>> Get(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            var employeeMapped = _mapper.Map<FactoryStaffDTO>(employee);
            return Ok(employeeMapped);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<FactoryStaffDTO>> Post([FromBody] FactoryStaffDTO employeeDto)
        {
            if (employeeDto == null)
            {
                return BadRequest();
            }

            var employee = _mapper.Map<FactoryStaffEntity>(employeeDto);
            await _employeeRepository.CreateAsync(employee);

            var createdEmployeeDto = _mapper.Map<FactoryStaffDTO>(employee);
            return CreatedAtAction(nameof(Get), new { id = createdEmployeeDto.Id }, createdEmployeeDto);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Put(int id, [FromBody] FactoryStaffDTO employeeDto)
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
        [Authorize(Roles = "Administrator")]
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

