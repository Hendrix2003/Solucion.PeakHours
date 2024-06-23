using Microsoft.EntityFrameworkCore;
using Radzen;
using SolucionPeakHours.Interfaces;
using SolucionPeakHours.Models;
using SolucionPeakHours.Persistence;

namespace SolucionPeakHours.Repositories
{
    public class ProgHourRepository : BaseCrudRepository<ProgHourEntity>, IProgHourRepository
    {
        private readonly CndDbContext _context;

        public ProgHourRepository(CndDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<ProgHourEntity>> GetProgHourByUserArea(string area)
        {
            var entities = await _context
                .ProgHours
                .Include(x => x.FactoryStaffEntity)
                .Where(x => x.FactoryStaffEntity!
                .Area!
                .Contains(area))
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            return entities;
        }
        public override async Task<List<ProgHourEntity>> GetAll()
        {
            var entities = await _context
               .ProgHours
               .Include(x => x.FactoryStaffEntity)
               .OrderByDescending(x => x.CreatedAt)
               .ToListAsync();

            return entities;
        }

        public async Task<List<ProgHourEntity>> GetProgHoursByEmployeeId(int employeeId)
        {
            var entities = await _context
                .ProgHours
                .Include(x => x.FactoryStaffEntity)
                .Where(x => x.FactoryStaffEntity!.Id == employeeId
                        && !x.ProgExpired)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            return entities;
        }
    }
}

/*var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;

            var p = entities
                .Where(x => x.ProgDate.Month == currentMonth
                       && x.ProgDate.Year == currentYear)
                .Sum(x => x.HourCant);*/