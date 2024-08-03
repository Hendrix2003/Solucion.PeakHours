using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using PeakHours_Blazor.Helpers;
using PeakHours_Blazor.Interfaces;
using Radzen;
using SolucionPeakHours.Shared.UserAccount;

namespace PeakHours_Blazor.Pages.User
{
    [Authorize(Roles = "Administrator, GerenteArea")]
    public partial class Index
    {
        [Inject] private IUserService _userService { get; set; }
        [Inject] private DialogService DialogService { get; set; }
        [Inject] private BeamAuthenticationStateProviderHelper BeamAuthenticationStateProviderHelper { get; set; }

        private bool HasPermission { get; set; }
        private bool IsAdmin { get; set; }
        private List<UserDTO> Users { get; set; }
        private async Task CheckEmergencyTimePermissionAsync(string userIdentityId)
        {
            var confirm = await SweetAlertHelper.ShowWarningAlert("Atención", "¿Estás seguro de modificar el permiso a este usuario de hora emergencial");

            if (confirm)
            {
                var result = await _userService.CheckEmergencyTimePermissionAsync(userIdentityId);

                if (result)
                {
                    await SweetAlertHelper.ShowSuccessAlert("Éxito", "Se ha modificado el permiso de hora emergencial al usuario");

                    await LoadData();
                }
                else
                {
                    await SweetAlertHelper.ShowErrorAlert("Error", "No se ha podido modificar el permiso de hora emergencial al usuario");
                }
            }
        }
        private async Task LoadData()
        {
            Users = await _userService.GetUsersAsync();
        }
        private async Task DeleteUser(string userId)
        {
            var confirm = await SweetAlertHelper.ShowWarningAlert("Atención", "¿Estás seguro de eliminar este usuario?");

            if (confirm)
            {
                await _userService.DeleteUser(userId);
                await LoadData();
            }
        }
        private async Task OpenFormDialog(string title, UserDTO? user = default)
        {
            var options = new DialogOptions() { Width = "auto", Height = "auto" };

            var dialog = await DialogService
                .OpenAsync<UserForm>
                ( title
                , parameters: new Dictionary<string, object>
                {
                    {
                        "UserParameter", user
                    }
                }, options);

            if (dialog is not null)
            {
                await LoadData();
            }
        }
        protected override async Task OnInitializedAsync()
        {
            var authState = await BeamAuthenticationStateProviderHelper.GetAuthenticationStateAsync();

            HasPermission = authState.User.IsInRole("Administrator") || authState.User.IsInRole("GerenteArea");
            IsAdmin = authState.User.IsInRole("Administrator");

            await LoadData();
        }
    }
}
