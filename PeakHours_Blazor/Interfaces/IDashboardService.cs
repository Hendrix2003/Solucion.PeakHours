using SolucionPeakHours.Shared.Dashboard;

namespace PeakHours_Blazor.Interfaces
{
    public interface IDashboardService
    {
        Task<List<StaticsResponse>> GetTotalHoursByArea(string area, int lastBy);

        Task<List<StaticsResponse>> GetTotalHoursByEmployee(int emplooyeeId, int lastBy);

        Task<List<StaticsResponse>> GetEmployeesWithMoreHours();

        Task<List<EmployeeDashboard>> GetEmployeesWithDetails();

    }
}
