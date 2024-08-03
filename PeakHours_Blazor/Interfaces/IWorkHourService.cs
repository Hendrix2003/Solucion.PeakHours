using SolucionPeakHours.Shared.CompletedHours;
using System.Net.Http.Json;

namespace PeakHours_Blazor.Interfaces
{
    public interface IWorkHourService
    {
        Task<List<WorkHourDTO>> GetAll();
        Task<List<WorkHourDTO>> GetWorkHoursByUserArea();
        Task<WorkHourDTO> GetByIdAsync(int id);
        Task<WorkHourDTO> CreateAsync(WorkHourDTO entity);
        Task<WorkHourDTO> UpdateAsync(WorkHourDTO entity);
        Task<bool> DeleteAsync(int id);

        Task<ApproveResponse> ApproveByAreaManager(int workHourId);
        Task<ApproveResponse> ApproveByFactoryManager(int workHourId);
    }
}
