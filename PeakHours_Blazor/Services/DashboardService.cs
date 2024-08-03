using PeakHours_Blazor.Interfaces;
using SolucionPeakHours.Shared.Dashboard;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace PeakHours_Blazor.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly HttpClient _httpClient;
        private readonly LocalStorageHelper _localStorageHelper;

        public DashboardService(HttpClient httpClient, LocalStorageHelper localStorageHelper)
        {
            _httpClient = httpClient;
            _localStorageHelper = localStorageHelper;
        }

        public async Task<List<StaticsResponse>> GetTotalHoursByArea(string area, int lastBy)
        {
            var token = await _localStorageHelper.GetItem("token");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = await _httpClient.GetAsync($"api/dashboard/getTotalHoursByArea?area={area}&lastByDay={lastBy}");

            var response = await request.Content.ReadFromJsonAsync<List<StaticsResponse>>();

            return response!;
        }

        public async Task<List<StaticsResponse>> GetTotalHoursByEmployee(int emplooyeeId, int lastBy)
        {
            var token = await _localStorageHelper.GetItem("token");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = await _httpClient.GetAsync($"api/dashboard/getTotalHoursByEmployee?emplooyeeId={emplooyeeId}&lastByDay={lastBy}");

            var response = await request.Content.ReadFromJsonAsync<List<StaticsResponse>>();

            return response!;
        }

        public async Task<List<StaticsResponse>> GetEmployeesWithMoreHours()
        {
            var token = await _localStorageHelper.GetItem("token");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = await _httpClient.GetAsync($"api/dashboard/getEmployeesWithMoreHours");

            var response = await request.Content.ReadFromJsonAsync<List<StaticsResponse>>();

            return response!;
        }

        public async Task<List<EmployeeDashboard>> GetEmployeesWithDetails()
        {
            var token = await _localStorageHelper.GetItem("token");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = await _httpClient.GetAsync($"api/dashboard/getEmployeesWithDetails");

            var response = await request.Content.ReadFromJsonAsync<List<EmployeeDashboard>>();

            return response!;
        }
    }
}
