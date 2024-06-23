

using SolucionPeakHours.Entities;

namespace SolucionPeakHours.Interfaces
{
    public interface IUserRepositoy
    {
        Task<bool> CheckEmergencyTimePermissionAsync(string userIdentityId);
        Task<UserIdentityEntity> GetUserById(string userId);
    }
}
