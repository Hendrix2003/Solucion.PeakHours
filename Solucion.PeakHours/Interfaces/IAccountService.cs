
using SolucionPeakHours.Entities;

namespace SolucionPeakHours.Interfaces
{
    public interface IAccountService
    {
        Task<List<UserIdentityEntity>> GetUsers();
        Task<List<string>> GetRoles();
        Task<UserIdentityEntity> GetUserById(string id);
        Task<UserIdentityEntity> Login(string username, string password);
        Task<UserIdentityEntity> Register(UserIdentityEntity user, string password);
        Task<bool> DeleteUser(string userId);
        Task<UserIdentityEntity> UpdateUser(UserIdentityEntity user, string password);
        Task<bool> AddRoleToUser(string userId, string role);
        Task<bool> CreateRole(string role);
    }
}
