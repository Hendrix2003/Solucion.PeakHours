using PeakHours_Blazor.Interfaces;
using SolucionPeakHours.Shared.FactoryStaff;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace PeakHours_Blazor.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly HttpClient _httpClient;
        private readonly LocalStorageHelper _localStorageHelper;

        public EmployeeService(HttpClient httpClient, LocalStorageHelper localStorageHelper)
        {
            _httpClient = httpClient;
            _localStorageHelper = localStorageHelper;
        }

        public async Task<List<string>> GetAreas()
        {
            var token = await _localStorageHelper.GetItem("token");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = await _httpClient.GetAsync("api/Employee/GetAreas");

            var response = await request.Content.ReadFromJsonAsync<List<string>>();

            return response;

        }

        public async Task<List<FactoryStaffDTO>> GetEmployeesByArea(string area)
        {
            var token = await _localStorageHelper.GetItem("token");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = await _httpClient.GetAsync($"api/Employee/GetEmployeesByArea?area={area}");

            var response = await request.Content.ReadFromJsonAsync<List<FactoryStaffDTO>>();

            return response!;
        }

        public async Task<List<FactoryStaffDTO>> GetAll()
        {
            var token = await _localStorageHelper.GetItem("token");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request  = await _httpClient.GetAsync("api/Employee/Get");

            var response = await request.Content.ReadFromJsonAsync<List<FactoryStaffDTO>>();

            return response!;
        }

        public async Task<FactoryStaffDTO> GetByIdAsync(int id)
        {
            var token = await _localStorageHelper.GetItem("token");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = await _httpClient.GetAsync($"api/Employee/{id}");

            var response = await request.Content.ReadFromJsonAsync<FactoryStaffDTO>();

            return response!;
        }

        public async Task<FactoryStaffDTO> UpdateAsync(FactoryStaffDTO entity)
        {
            var token = await _localStorageHelper.GetItem("token");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = await _httpClient.PutAsJsonAsync($"api/Employee/{entity.Id}", entity);

            var response = await request.Content.ReadFromJsonAsync<FactoryStaffDTO>();

            return response!;
        }

        public async Task<FactoryStaffDTO> CreateAsync(FactoryStaffDTO entity)
        {
            var token = await _localStorageHelper.GetItem("token");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = await _httpClient.PostAsJsonAsync("api/Employee", entity);

            var response = await request.Content.ReadFromJsonAsync<FactoryStaffDTO>();

            return response!;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var token = await _localStorageHelper.GetItem("token");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = await _httpClient.DeleteAsync($"api/Employee/{id}");

            return request.StatusCode == HttpStatusCode.OK;
        }

       
    }
}
