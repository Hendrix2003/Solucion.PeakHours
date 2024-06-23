using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SolucionPeakHours.Entities;
using SolucionPeakHours.Interfaces;

namespace SolucionPeakHours.Repositories
{
    public class UserRepositoy : IUserRepositoy
    {
        private readonly UserManager<UserIdentityEntity> _userManager;

        public UserRepositoy(UserManager<UserIdentityEntity> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> CheckEmergencyTimePermissionAsync(string userIdentityId)
        {
            var user = await _userManager.FindByIdAsync(userIdentityId);

            user!.AllowEmergencyTime = !user.AllowEmergencyTime;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return false;
            }

            return true;
        }

        public async Task<UserIdentityEntity> GetUserById(string userId)
        {
            var user = await _userManager
                .Users
                .Include(x => x.FactoryStaffEntity)
                .Where(x => x.Id == userId)
                .FirstOrDefaultAsync();

            return user!;
        }
    }
}
