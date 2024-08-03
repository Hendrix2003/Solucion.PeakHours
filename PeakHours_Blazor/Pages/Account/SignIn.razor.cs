using Microsoft.AspNetCore.Components;
using PeakHours_Blazor.Helpers;
using PeakHours_Blazor.Interfaces;
using Radzen;
using SolucionPeakHours.Shared.UserAccount;

namespace PeakHours_Blazor.Pages.Account
{
    public partial class SignIn
    {
        [Inject] private NavigationManager _navigationManager { get; set; }
        [Inject] private IUserService _userService { get; set; }
        [Inject] private LocalStorageHelper _localStorageHelper { get; set; }

        private bool isLoading = false;

        protected override async Task OnInitializedAsync()
        {
            
        }

        private async Task HandleLogin(LoginArgs args)
        {
            isLoading = true;

            var loginPayload = new LoginDTO
            {
                UserName = args.Username,
                Pwd = args.Password
            };

            var loginResult = await _userService.Login(loginPayload.UserName, loginPayload.Pwd);

            if(loginResult is not null)
            {
                _navigationManager.NavigateTo("/");

                await _localStorageHelper.SetItem("token", loginResult.Token!);
            }
            else
            {
                await SweetAlertHelper.ShowErrorAlert("Credenciales incorrectas", "Usuario o contraseña incorrectos");
            }

            isLoading = false;
        }
    }
}
