using Microsoft.AspNetCore.Components.Authorization;
using PeakHours_Blazor.Interfaces;
using System.Security.Claims;

namespace PeakHours_Blazor.Helpers
{
    public class BeamAuthenticationStateProviderHelper : AuthenticationStateProvider
    {
        private readonly IUserService _userService;
        
        public BeamAuthenticationStateProviderHelper(IUserService userService)
        {
            _userService = userService;
        }

        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var currentUser = await _userService.GetCurrentUserState();

            if (!currentUser.IsAuthenticated)
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            var identity = new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.Name, currentUser.FullName),
                new Claim(ClaimTypes.Role, currentUser.Role),
                new Claim("Area", currentUser.Area),
            }, "Auth");

            var user = new ClaimsPrincipal(identity);

            return await Task.FromResult(new AuthenticationState(user));
        }
    }
}
