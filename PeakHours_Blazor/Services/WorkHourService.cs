using DocumentFormat.OpenXml.Office2010.Excel;
using PeakHours_Blazor.Interfaces;
using SolucionPeakHours.Shared.CompletedHours;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace PeakHours_Blazor.Services
{
    public class WorkHourService : IWorkHourService
    {
        private readonly HttpClient _httpClient;
        private readonly LocalStorageHelper _localStorageHelper;

        public WorkHourService(HttpClient httpClient, LocalStorageHelper localStorageHelper)

        {
            _httpClient = httpClient;
            _localStorageHelper = localStorageHelper;
        }

        public async Task<List<WorkHourDTO>> GetWorkHoursByUserArea()
        {
            var token = await _localStorageHelper.GetItem("token");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = await _httpClient.GetAsync("api/WorkHour/GetWorkHoursByUserArea");

            if (!request.IsSuccessStatusCode)
                throw new Exception("Error al obtener los datos");

            var response = await request.Content.ReadFromJsonAsync<List<WorkHourDTO>>();

            return response!;
        }

        public async Task<List<WorkHourDTO>> GetAll()
        {
            var token = await _localStorageHelper.GetItem("token");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = await _httpClient.GetAsync("api/WorkHour/GetAll");

            if (!request.IsSuccessStatusCode)
                throw new Exception("Error al obtener los datos");

            var response = await request.Content.ReadFromJsonAsync<List<WorkHourDTO>>();

            return response!;
        }

        public async Task<WorkHourDTO> GetByIdAsync(int id)
        {
            var token = await _localStorageHelper.GetItem("token");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = await _httpClient.GetAsync($"api/WorkHour/{id}");

            if (!request.IsSuccessStatusCode)
                throw new Exception("Error al obtener los datos");

            var response = await request.Content.ReadFromJsonAsync<WorkHourDTO>();

            return response!;
        }

        public async Task<WorkHourDTO> UpdateAsync(WorkHourDTO entity)
        {
            var token = await _localStorageHelper.GetItem("token");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = await _httpClient.PutAsJsonAsync($"api/WorkHour/{entity.Id}", entity);

            if (!request.IsSuccessStatusCode)
                throw new Exception("Error al actualizar");

            var response = await request.Content.ReadFromJsonAsync<WorkHourDTO>();

            return response!;
        }

        public async Task<WorkHourDTO> CreateAsync(WorkHourDTO entity)
        {
            var token = await _localStorageHelper.GetItem("token");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = await _httpClient.PostAsJsonAsync("api/WorkHour", entity);

            if (!request.IsSuccessStatusCode)
                throw new Exception("Error al crear");

            var response = await request.Content.ReadFromJsonAsync<WorkHourDTO>();

            return response!;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var token = await _localStorageHelper.GetItem("token");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = await _httpClient.DeleteAsync($"api/WorkHour/Delete?IdPerson={id}");

            if (!request.IsSuccessStatusCode)
                throw new Exception("Error al eliminar");

            return request.IsSuccessStatusCode;
        }

        public async Task<ApproveResponse> ApproveByAreaManager(int workHourId)
        {
            var token = await _localStorageHelper.GetItem("token");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = await _httpClient.PostAsJsonAsync($"api/WorkHour/ApproveByAreaManager?workHourId={workHourId}", workHourId);

            if (!request.IsSuccessStatusCode)
                throw new Exception("Error al obtener los datos");

            var response = await request.Content.ReadFromJsonAsync<ApproveResponse>();

            return response!;
        }

        public async Task<ApproveResponse> ApproveByFactoryManager(int workHourId)
        {
            var token = await _localStorageHelper.GetItem("token");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = await _httpClient.PostAsJsonAsync($"api/WorkHour/ApproveByFactoryManager?workHourId={workHourId}", workHourId);

            if (!request.IsSuccessStatusCode)
                throw new Exception("Error al obtener los datos");

            var response = await request.Content.ReadFromJsonAsync<ApproveResponse>();

            return response!;
        }
    }
}
