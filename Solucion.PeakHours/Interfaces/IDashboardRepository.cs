using SolucionPeakHours.Shared.Dashboard;

namespace SolucionPeakHours.Repositories
{
    public interface IDashboardRepository
    {
        Task<List<EmployeeDashboard>> GetEmployeesWithDetails();
        Task<List<StaticsResponse>> GetTotalHoursByArea(string area, int lastBy);
        Task<List<StaticsResponse>> GetTotalHoursByEmployee(int emplooyeeId, int lastBy);
        Task<List<StaticsResponse>> GetEmployeesWithMoreHours();

        Task<List<EmployeeDashboard>> GetEmployeesWithDetailsByUserArea(string userArea);
        Task<List<StaticsResponse>> GetTotalHoursByUserArea(int lastBy, string userArea);
        Task<List<StaticsResponse>> GetTotalHoursByEmployeeByUserArea(int emplooyeeId, int lastBy, string userArea);
        Task<List<StaticsResponse>> GetEmployeesWithMoreHoursByUserArea(string userArea);
    }
}
