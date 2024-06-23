using SolucionPeakHours.Models;

namespace SolucionPeakHours.Interfaces
{
    public interface IProgHourRepository : IBaseCrudRepository<ProgHourEntity>
    {
        public Task<List<ProgHourEntity>> GetProgHourByUserArea(string area);
        public Task<List<ProgHourEntity>> GetProgHoursByEmployeeId(int employeeId);

    }
}
