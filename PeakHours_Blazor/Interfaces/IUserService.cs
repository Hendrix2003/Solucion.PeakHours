using SolucionPeakHours.Shared.UserAccount;

namespace PeakHours_Blazor.Interfaces
{
    public interface IUserService
    {
        Task<List<string>> GetRoles();

        Task<bool> CheckEmergencyTimePermissionAsync(string userIdentityId);
        Task<UserDTO> GetUserById(string userId);
        Task<UserStateAuthResponse> GetCurrentUserState();
        Task<List<UserDTO>> GetUsersAsync();
        Task<AuthResponse> Login(string username, string password);
        Task<AuthResponse> Register(RegisterDTO user, string password);
        Task<bool> AddRoleToUser(string userId, string role);
        Task<bool> CreateRole(string role);
        Task<UserDTO> UpdateUser(UserDTO user, string password);
        Task<bool> DeleteUser(string userId);
    }
}

