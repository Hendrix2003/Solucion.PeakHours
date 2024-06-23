using Microsoft.EntityFrameworkCore;
using SolucionCND.Shared.Dashboard;
using SolucionPeakHours.Persistence;
using SolucionPeakHours.Shared.CompletedHours;
using SolucionPeakHours.Shared.Dashboard;
using SolucionPeakHours.Shared.ProgHours;

namespace SolucionPeakHours.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly CndDbContext _context;

        public DashboardRepository(CndDbContext context)
        {
            _context = context;
        }

        public async Task<List<EmployeeDashboard>> GetEmployeesWithDetails()
        {
            var query = _context.FactoryStaffs
                .Include(x => x.ProgHours)
                .ThenInclude(x => x.WorkHours)
                .AsQueryable();

            var employeesWithWorkHoursAny = query
                .Where(x => x.ProgHours.Any(x => x.WorkHours.Any()))
                .AsQueryable();

            var employeesDashboard = employeesWithWorkHoursAny
                .Select(x => new EmployeeDashboard
                {
                    Id = (int)x.Id!,
                    FullName = x.FullName,
                    Manager = x.Manager,
                    Area = x.Area,
                    SubArea = x.SubArea,
                    Position = x.Position,
                    Line = x.Line,
                    TotalHoursPerMonth = x.TotalHoursPerMonth,

                    ProgHours = x.ProgHours.Select(x => new ProgHourDTO
                    {
                        Id = (int)x.Id!,
                        HourCant = x.HourCant,
                        Reasons = x.Reasons,
                        ProgDate = x.ProgDate,
                        CreatedAt = x.CreatedAt,
                    }).ToList(),

                    WorkHours = x.ProgHours.SelectMany(x => x.WorkHours.Where(x => x.AreaManagerApproved && x.FactoryManagerApproved))
                                           .Select(x => new WorkHourDTO
                                           {
                                               Id = (int)x.Id!,
                                               HoursWorked = x.HoursWorked,
                                               HourType = x.HourType,
                                               ReasonsRelize = x.ReasonsRelize,
                                               FechaOld = x.FechaOld,
                                               CreatedAt = x.CreatedAt,
                                               AreaManagerApproved = x.AreaManagerApproved,
                                               FactoryManagerApproved = x.FactoryManagerApproved,
                                           }).ToList()
                });

            var employeesDashboardResponse = await employeesDashboard.ToListAsync();

            return employeesDashboardResponse;
        }
        public async Task<List<StaticsResponse>> GetTotalHoursByArea(string area, int lastByDay)
        {
            var query = await _context.WorkHours
                .Include(x => x.ProgHourEntity)
                .ThenInclude(x => x.FactoryStaffEntity)
                .Where(x => x.ProgHourEntity!
                             .FactoryStaffEntity!
                             .Area!
                             .Contains(area)
                             && x.AreaManagerApproved && x.FactoryManagerApproved)
                .OrderByDescending(x => x.CreatedAt)
                .GroupBy(x => x.CreatedAt)
                .Select(x => new StaticsResponse
                {
                    Cant = x.Sum(x => (int)x.HoursWorked!),
                    Date = x.Key!.Value.ToString("dd/MM/yyyy")
                })
                .Take(lastByDay)
                .ToListAsync();

            return query;
        }
        public async Task<List<StaticsResponse>> GetEmployeesWithMoreHours()
        {
            var query = await _context.WorkHours
                .Where(x => x.AreaManagerApproved && x.FactoryManagerApproved)
                .Include(x => x.ProgHourEntity)
                .ThenInclude(x => x.FactoryStaffEntity)
                .GroupBy(x => x.ProgHourEntity!.FactoryStaffEntity!.Id)
                .Select(x => new StaticsResponse
                {
                    Cant = x.Sum(x => (int)x.HoursWorked!),
                    Description = $"{x.Key} - {x.Select(x => x.ProgHourEntity!.FactoryStaffEntity!.Area).FirstOrDefault()}"
                })
                .OrderByDescending(x => x.Cant)
                .Take(5)
                .ToListAsync();

            return query;
        }
        public async Task<List<StaticsResponse>> GetTotalHoursByEmployee(int emplooyeeId, int lastBy)
        {
            var query = await _context.WorkHours
                .Include(x => x.ProgHourEntity)
                .ThenInclude(x => x.FactoryStaffEntity)
                .Where(x => x.ProgHourEntity!.FactoryStaffEntity!.Id == emplooyeeId)
                .OrderByDescending(x => x.CreatedAt)
                .GroupBy(x => x.CreatedAt)
                .Select(x => new StaticsResponse
                {
                    Cant = x.Sum(x => (int)x.HoursWorked!),
                    Date = x.Key!.Value.ToString("dd/MM/yyyy")
                })
                .Take(lastBy)
                .ToListAsync();

            return query;
        }

        public async Task<List<StaticsResponse>> GetTotalHoursByEmployeeByUserArea(int emplooyeeId, int lastBy, string area)
        {
            var query = await _context.WorkHours
                .Include(x => x.ProgHourEntity)
                .ThenInclude(x => x.FactoryStaffEntity)
                .Where(x => x.ProgHourEntity!.FactoryStaffEntity!.Id == emplooyeeId
                        && x.ProgHourEntity.FactoryStaffEntity.Area.Contains(area))
                .OrderByDescending(x => x.CreatedAt)
                .GroupBy(x => x.CreatedAt)
                .Select(x => new StaticsResponse
                {
                    Cant = x.Sum(x => (int)x.HoursWorked!),
                    Date = x.Key!.Value.ToString("dd/MM/yyyy")
                })
                .Take(lastBy)
                .ToListAsync();

            return query;
        }
        public async Task<List<EmployeeDashboard>> GetEmployeesWithDetailsByUserArea(string area)
        {
            var query = _context.FactoryStaffs
                .Where(x => x.Area!.Contains(area))
                .Include(x => x.ProgHours)
                .ThenInclude(x => x.WorkHours)
                .AsQueryable();

            var employeesWithWorkHoursAny = query
                .Where(x => x.ProgHours.Any(x => x.WorkHours.Any()))
                .AsQueryable();

            var employeesDashboard = employeesWithWorkHoursAny
                .Select(x => new EmployeeDashboard
                {
                    Id = (int)x.Id!,
                    FullName = x.FullName,
                    Manager = x.Manager,
                    Area = x.Area,
                    SubArea = x.SubArea,
                    Position = x.Position,
                    Line = x.Line,
                    TotalHoursPerMonth = x.TotalHoursPerMonth,

                    ProgHours = x.ProgHours.Select(x => new ProgHourDTO
                    {
                        Id = (int)x.Id!,
                        HourCant = x.HourCant,
                        Reasons = x.Reasons,
                        ProgDate = x.ProgDate,
                        CreatedAt = x.CreatedAt,
                    }).ToList(),

                    WorkHours = x.ProgHours.SelectMany(x => x.WorkHours.Where(x => x.AreaManagerApproved && x.FactoryManagerApproved))
                                           .Select(x => new WorkHourDTO
                                           {
                                               Id = (int)x.Id!,
                                               HoursWorked = x.HoursWorked,
                                               HourType = x.HourType,
                                               ReasonsRelize = x.ReasonsRelize,
                                               FechaOld = x.FechaOld,
                                               CreatedAt = x.CreatedAt,
                                               AreaManagerApproved = x.AreaManagerApproved,
                                               FactoryManagerApproved = x.FactoryManagerApproved,
                                           }).ToList()
                });

            var employeesDashboardResponse = await employeesDashboard.ToListAsync();

            return employeesDashboardResponse;
        }
        public async Task<List<StaticsResponse>> GetEmployeesWithMoreHoursByUserArea(string area)
        {
            var query = await _context.WorkHours
                .Include(x => x.ProgHourEntity)
                .ThenInclude(x => x.FactoryStaffEntity)
                .Where(x => x.ProgHourEntity!
                             .FactoryStaffEntity!
                             .Area!
                             .Contains(area)
                       && x.AreaManagerApproved && x.FactoryManagerApproved)
                .GroupBy(x => x.ProgHourEntity!.FactoryStaffEntity!.Id)
                .Select(x => new StaticsResponse
                {
                    Cant = x.Sum(x => (int)x.HoursWorked!),
                    Description = $"{x.Key} - {x.Select(x => x.ProgHourEntity!.FactoryStaffEntity!.Area).FirstOrDefault()}"
                })
                .OrderByDescending(x => x.Cant)
                .Take(5)
                .ToListAsync();

            return query;
        }
        public async Task<List<StaticsResponse>> GetTotalHoursByUserArea(int lastByDay, string userArea)
        {
            var query = await _context.WorkHours
                .Include(x => x.ProgHourEntity)
                .ThenInclude(x => x.FactoryStaffEntity)
                .Where(x => x.ProgHourEntity!
                             .FactoryStaffEntity!
                             .Area!
                             .Contains(userArea))
                .OrderByDescending(x => x.CreatedAt)
                .GroupBy(x => x.CreatedAt)
                .Select(x => new StaticsResponse
                {
                    Cant = x.Sum(x => (int)x.HoursWorked!),
                    Date = x.Key!.Value.ToString("dd/MM/yyyy")
                })
                .Take(lastByDay)
                .ToListAsync();

            return query;
        }

    }
}
