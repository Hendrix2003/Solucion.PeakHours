
using SolucionCND.Shared.CompletedHours;
using SolucionPeakHours.Models;

namespace SolucionPeakHours.Interfaces
{
    public interface IWorkHourRepository : IBaseCrudRepository<WorkHourEntity>
    {
        Task<ApproveResponse> ApproveByAreaManager(int workHourId);
        Task<ApproveResponse> ApproveByFactoryManager(int workHourId);
        Task<List<WorkHourEntity>> GetWorkHoursByUserArea(string area);
        Task<WorkHourEntity> GetByNameAsync(string name);
    }
}
