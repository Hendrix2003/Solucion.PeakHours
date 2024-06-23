using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SolucionPeakHours.Entities;
using SolucionPeakHours.Interfaces;

namespace SolucionPeakHours.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<UserIdentityEntity> _userManager;
        private readonly SignInManager<UserIdentityEntity> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountService(UserManager<UserIdentityEntity> userManager,
                           SignInManager<UserIdentityEntity> signInManager,
                           RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<bool> AddRoleToUser(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            var result = await _userManager.AddToRoleAsync(user, role);

            if (!result.Succeeded)
            {
                throw new Exception("Role addition failed");
            }

            return true;
        }

        public async Task<bool> CreateRole(string role)
        {
            var result = await _roleManager.CreateAsync(new IdentityRole(role));

            if (!result.Succeeded)
            {
                throw new Exception("Role creation failed");
            }

            return true;
        }

        public async Task<List<string>> GetRoles()
        {
            return await _roleManager
                .Roles
                .Select(x => x.Name!)
                .ToListAsync();
        }

        public async Task<List<UserIdentityEntity>> GetUsers()
        {
            var users = await _userManager
                .Users
                .Include(u => u.FactoryStaffEntity)
                .ToListAsync();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                user.Role = roles.FirstOrDefault()!;
            }

            return users;
        }

        public async Task<UserIdentityEntity> Login(string username, string password)
        {
            var user = await _userManager.FindByEmailAsync(username);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);

            if (!result.Succeeded)
            {
                throw new Exception("Invalid password");
            }

            return user;
        }

        public async Task<UserIdentityEntity> Register(UserIdentityEntity user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                throw new Exception("User creation failed");
            }

            return user;
        }

        public async Task<UserIdentityEntity> UpdateUser(UserIdentityEntity user, string password)
        {
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                throw new Exception("User update failed");
            }


            return user;
        }

        public async Task<bool> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                throw new Exception("User deletion failed");
            }

            return true;
        }

        public async Task<UserIdentityEntity> GetUserById(string userId)
        {
            var user = await _userManager
                .Users
                .Include(x => x.FactoryStaffEntity)
                .FirstOrDefaultAsync(x => x.Id == userId);

            if(user is null)
            {
                throw new Exception("User not found");
            }

            var userRole = await _userManager.GetRolesAsync(user);

            user.Role = userRole.FirstOrDefault()!;

            return user;
        }
    }
}
