
using SolucionPeakHours.Shared.FactoryStaff;

namespace PeakHours_Blazor.Interfaces
{
    public interface IEmployeeService
    {
        Task<List<FactoryStaffDTO>> GetAll();
        Task<List<FactoryStaffDTO>> GetEmployeesByArea(string area);
        Task<FactoryStaffDTO> GetByIdAsync(int id);
        Task<FactoryStaffDTO> CreateAsync(FactoryStaffDTO entity);
        Task<FactoryStaffDTO> UpdateAsync(FactoryStaffDTO entity);
        Task<List<string>> GetAreas();
        Task<bool> DeleteAsync(int id);
    }
}
