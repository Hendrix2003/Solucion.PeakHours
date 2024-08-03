using Microsoft.EntityFrameworkCore;
using SolucionPeakHours.Shared.CompletedHours;
using SolucionPeakHours.Interfaces;
using SolucionPeakHours.Models;
using SolucionPeakHours.Persistence;

namespace SolucionPeakHours.Repositories
{
    public class WorkHourRepository : BaseCrudRepository<WorkHourEntity>, IWorkHourRepository
    {
        private readonly CndDbContext _context;

        public WorkHourRepository(CndDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<WorkHourEntity> GetByIdAsync(int id)
        {
            var entity = await _context
                .WorkHours
                .Include(x => x.ProgHourEntity)
                .ThenInclude(x => x.FactoryStaffEntity)
                .FirstOrDefaultAsync(e => e.Id == id);

            return entity!;
        }

        public override async Task<List<WorkHourEntity>> GetAll()
        {
            return await _context
                .WorkHours
                .Include(x => x.ProgHourEntity)
                .ThenInclude(x => x.FactoryStaffEntity)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<WorkHourEntity> GetByNameAsync(string name)
        {
            return await _context
                .WorkHours
                .Include(x => x.ProgHourEntity)
                .ThenInclude(x => x.FactoryStaffEntity)
                .FirstOrDefaultAsync(e => e.ProgHourEntity
                .FactoryStaffEntity
                .FullName == name);
        }

        public async Task<List<WorkHourEntity>> GetWorkHoursByUserArea(string area)
        {
            var entities = await _context
                .WorkHours
                .Include(x => x.ProgHourEntity)
                .ThenInclude(x => x!.FactoryStaffEntity)
                .Where(x => x.ProgHourEntity!
                .FactoryStaffEntity!
                .Area!.Contains(area))
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            return entities;
        }

        public async Task<ApproveResponse> ApproveByAreaManager(int workHourId)
        {
            var response = new ApproveResponse
            {
                IsSuccess = true,
                Message = "Aprobado"
            };

            var entity = await _context
                .WorkHours
                .FirstOrDefaultAsync(e => e.Id == workHourId);

            if (entity == null)
            {
                response.IsSuccess = false;
                response.Message = "No se encontró la hora laborada";
                return response;
            }

            entity.AreaManagerApproved = !entity.AreaManagerApproved;
            await _context.SaveChangesAsync();

            return response;
        }

        public async Task<ApproveResponse> ApproveByFactoryManager(int workHourId)
        {
            var response = new ApproveResponse
            {
                IsSuccess = true,
                Message = "Aprobado"
            };

            var entity = await _context
                .WorkHours
                .FirstOrDefaultAsync(e => e.Id == workHourId);

            if (entity == null)
            {
                response.IsSuccess = false;
                response.Message = "No se encontró la hora laborada";
                return response;
            }

            if(entity.AreaManagerApproved == false)
            {
                response.IsSuccess = false;
                response.Message = "El área no ha aprobado la hora laborada";
                return response;
            }

            entity.FactoryManagerApproved = !entity.FactoryManagerApproved;
            await _context.SaveChangesAsync();

            return response;
        }
    }
}
