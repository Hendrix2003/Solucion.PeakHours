using Hangfire;
using Microsoft.EntityFrameworkCore;
using Solucion.PeakHours;
using SolucionPeakHours.Interfaces;
using SolucionPeakHours.Persistence;

namespace SolucionPeakHours.Services
{
    public class BackgroundTaskService : IBackgroundTaskService
    {
        private readonly CndDbContext _context;

        public BackgroundTaskService(CndDbContext context)
        {
            _context = context;
        }

        public async Task ExpireEntitiesTask()
        {
            var currentYear = DateTime.Now.Year;
            var currentMonth = DateTime.Now.Month;
            var expirationDay = DateTime.Now.AddDays(3).Day;
            var currentHour = DateTime.Now.Hour;

            var entities = await _context
                .ProgHours
                .Where(x => x.ProgDate.Year == currentYear
                         && x.ProgDate.Month == currentMonth
                         && x.ProgDate.Hour == currentHour
                         && x.ProgDate.Day == expirationDay
                         && !x.ProgExpired)
                .ToListAsync();

            foreach (var entity in entities)
            {
                entity.ProgExpired = true;
            }

            _context.UpdateRange(entities);
            await _context.SaveChangesAsync();
        }

        public async Task ExpireEntitiesTaskJob()
        {
            RecurringJob.AddOrUpdate("ExpireEntitiesTask"
                            ,() => ExpireEntitiesTask(), "*/20 * * * *");

            await Task.CompletedTask;
        }


        public async Task RestartTotalHourPerMonth()
        {
            _context
                .FactoryStaffs
                .FromSqlRaw(Queries.RestartTotalHourPerMonthQuery);

            await _context.SaveChangesAsync();
        }

        public async Task RestartTotalHourPerMonthJob()
        {
            RecurringJob.AddOrUpdate("RestartTotalHourPerMonth"
                                           ,() => RestartTotalHourPerMonth(), "0 0 1 * *");

            await Task.CompletedTask;
        }
    }
}
