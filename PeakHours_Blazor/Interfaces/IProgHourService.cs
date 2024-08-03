using SolucionPeakHours.Shared.ProgHours;

namespace PeakHours_Blazor.Interfaces
{
    public interface IProgHourService
    {
        Task<List<ProgHourDTO>> GetProgHours();
        Task<List<ProgHourDTO>> GetProgHoursByEmployeeId(int employeeId);
        Task<ProgHourDTO> GetByIdAsync(int id);
        Task<ProgHourDTO> CreateAsync(ProgHourDTO entity);
        Task<ProgHourDTO> UpdateAsync(ProgHourDTO entity);
        Task<bool> DeleteAsync(int id);
    }
}
