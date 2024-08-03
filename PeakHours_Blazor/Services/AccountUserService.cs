using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components;
using SolucionPeakHours.Shared.UserAccount;
using PeakHours_Blazor.Interfaces;
using PeakHours_Blazor.Helpers;

namespace PeakHours_Blazor.Services
{
    public class AccountUserService : AuthenticationStateProvider, IUserService
    {
        private readonly HttpClient _httpClient;
        private readonly LocalStorageHelper _localStorageHelper;
        private readonly NavigationManager _navigationManager;

        public AccountUserService(HttpClient httpClient
                                 , NavigationManager navigationManager
                                 , LocalStorageHelper localStorageHelper)
        {
            _navigationManager = navigationManager;
            _httpClient = httpClient;
            _localStorageHelper = localStorageHelper;
        }

        public async Task<bool> CheckEmergencyTimePermissionAsync(string userIdentityId)
        {
            var token = await _localStorageHelper.GetItem("token");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = await _httpClient.GetAsync($"api/UserAccount/checkEmergencyTimePermission?userId={userIdentityId}");

            var response = await request.Content.ReadFromJsonAsync<bool>();

            return response!;
        }

        public async Task<bool> AddRoleToUser(string userId, string role)
        {
            var token = await _localStorageHelper.GetItem("token");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var addRoleToUserDTO = new AddRoleToUserDTO
            {
                UserId = userId,
                Role = role
            };

            var request = await _httpClient.PostAsJsonAsync("api/UserAccount/addRoleToUser", addRoleToUserDTO);

            var response = await request.Content.ReadFromJsonAsync<bool>();

            return response!;
        }

        public async Task<bool> CreateRole(string role)
        {
            var token = await _localStorageHelper.GetItem("token");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = await _httpClient.PostAsJsonAsync("api/UserAccount/createRole", role);

            var response = await request.Content.ReadFromJsonAsync<bool>();

            return response!;
        }

        public async Task<bool> DeleteUser(string userId)
        {
            var token = await _localStorageHelper.GetItem("token");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = await _httpClient.DeleteAsync($"api/UserAccount/deleteUser?userId={userId}");

            var response = await request.Content.ReadFromJsonAsync<bool>();

            return response;
        }

        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _localStorageHelper.GetItem("token");

            if (string.IsNullOrEmpty(token))
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            return AuthenticationStateProviderHelper.BuildAuthenticationState(token);
        }

        public async Task<UserStateAuthResponse> GetCurrentUserState()
        {
            try
            {
                var token = await _localStorageHelper.GetItem("token");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var request = await _httpClient.GetAsync("api/UserAccount/getCurrentUserState");

                var response = await request.Content.ReadFromJsonAsync<UserStateAuthResponse>();

                return response!;
            }
            catch (Exception)
            {
                _navigationManager.NavigateTo("/account/signin");
                throw;
            }
        }

        public async Task<List<UserDTO>> GetUsersAsync()
        {
            var token = await _localStorageHelper.GetItem("token");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = await _httpClient.GetAsync("api/UserAccount/getUsers");

            var response = await request.Content.ReadFromJsonAsync<List<UserDTO>>();

            return response!;
        }

        public async Task<AuthResponse> Login(string username, string password)
        {
            var loginDTO = new LoginDTO
            {
                UserName = username,
                Pwd = password
            };

            var request = await _httpClient.PostAsJsonAsync("api/UserAccount/login", loginDTO);

            if (!request.IsSuccessStatusCode)
            {
                throw new Exception("Usuario o contraseña incorrectos");
            }

            var response = await request.Content.ReadFromJsonAsync<AuthResponse>();

            await _localStorageHelper.SetItem("token", response!.Token!);

            var authState = AuthenticationStateProviderHelper.BuildAuthenticationState(response!.Token!);

            NotifyAuthenticationStateChanged(Task.FromResult(authState));

            return response;
        }

        public async Task<AuthResponse> Register(RegisterDTO user, string password)
        {
            var token = await _localStorageHelper.GetItem("token");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = await _httpClient.PostAsJsonAsync("api/UserAccount/register", user);

            var response = await request.Content.ReadFromJsonAsync<AuthResponse>();

            return response!;
        }

        public async Task<UserDTO> UpdateUser(UserDTO user, string password)
        {
            var token = await _localStorageHelper.GetItem("token");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = await _httpClient.PutAsJsonAsync("api/UserAccount/updateUser", user);

            var response = await request.Content.ReadFromJsonAsync<UserDTO>();

            return response!;
        }

        public async Task<UserDTO> GetUserById(string userId)
        {
            var token = await _localStorageHelper.GetItem("token");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = await _httpClient.GetAsync($"api/UserAccount/getUserById?userId={userId}");

            var response = await request.Content.ReadFromJsonAsync<UserDTO>();

            return response!;
        }

        public async Task<List<string>> GetRoles()
        {
            var token = await _localStorageHelper.GetItem("token");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = await _httpClient.GetAsync("api/UserAccount/getRoles");

            var response = await request.Content.ReadFromJsonAsync<List<string>>();

            return response!;
        }
    }
}

