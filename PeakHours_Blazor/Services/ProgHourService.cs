using Microsoft.Extensions.Configuration;
using PeakHours_Blazor.Interfaces;
using SolucionPeakHours.Shared.ProgHours;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace PeakHours_Blazor.Services
{
    public class ProgHourService : IProgHourService
    {
        private readonly HttpClient _httpClient;
        private readonly LocalStorageHelper _localStorageService;

        public ProgHourService(HttpClient httpClient, LocalStorageHelper localStorageService)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
        }

        public async Task<List<ProgHourDTO>> GetProgHours()
        {
            var token = await _localStorageService.GetItem("token");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = await _httpClient.GetAsync($"api/ProgHour/Get");

            if (!request.IsSuccessStatusCode)
            {
                throw new Exception("Error al cargar los datos");
            }

            var response = await request.Content.ReadFromJsonAsync<List<ProgHourDTO>>();

            return response!;
        }

        public async Task<ProgHourDTO> GetByIdAsync(int id)
        {
            var token = await _localStorageService.GetItem("token");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetFromJsonAsync<ProgHourDTO>($"api/ProgHour/{id}");

            return response!;
        }

        public async Task<ProgHourDTO> UpdateAsync(ProgHourDTO entity)
        {
            var token = await _localStorageService.GetItem("token");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = await _httpClient.PutAsJsonAsync($"api/ProgHour/{entity.Id}", entity);

            if (!request.IsSuccessStatusCode)
            {
                throw new Exception("Error al actualizar");   
            }

            var response =  await request.Content.ReadFromJsonAsync<ProgHourDTO>();

            return response!;
        }

        public async Task<ProgHourDTO> CreateAsync(ProgHourDTO entity)
        {
            var token = await _localStorageService.GetItem("token");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = await _httpClient.PostAsJsonAsync("api/ProgHour", entity);

            request.EnsureSuccessStatusCode();

            var response = await request.Content.ReadFromJsonAsync<ProgHourDTO>();

            return response!;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var token = await _localStorageService.GetItem("token");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = await _httpClient.DeleteAsync($"api/ProgHour/Delete?IdPerson={id}");

            return request.IsSuccessStatusCode;
        }

        public async Task<List<ProgHourDTO>> GetProgHoursByEmployeeId(int employeeId)
        {
            var token = await _localStorageService.GetItem("token");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = await _httpClient
                    .GetAsync($"api/ProgHour/GetProgHoursByEmployeeId?employeeId={employeeId}");

            var response = await request.Content.ReadFromJsonAsync<List<ProgHourDTO>>();

            return response!;
        }
    }
}

