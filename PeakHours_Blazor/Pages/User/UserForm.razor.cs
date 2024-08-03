using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using PeakHours_Blazor.Helpers;
using PeakHours_Blazor.Interfaces;
using Radzen;
using SolucionPeakHours.Shared.FactoryStaff;
using SolucionPeakHours.Shared.UserAccount;

namespace PeakHours_Blazor.Pages.User
{
    [Authorize(Roles = "Administrator")]
    public partial class UserForm
    {
        [Inject] public IUserService usuarioService { get; set; }
        [Inject] public IEmployeeService empleadoService { get; set; }
        [Inject] public DialogService DialogService { get; set; }

        [Parameter] public UserDTO? UserParameter { get; set; }
        private List<FactoryStaffDTO> Employees { get; set; } = new List<FactoryStaffDTO>();
        private List<string> Roles { get; set; } = new List<string>();
        private RegisterDTO RegistrationModel = new RegisterDTO();
        private string btnText = "";

        private bool IsEdit => UserParameter is not null;

        protected override async Task OnInitializedAsync()
        {
            await LoadEmployeeData();
            await LoadRoles();

            btnText = UserParameter is null ? "Registrar" : "Actualizar";

            if (UserParameter is not null)
            {
                RegistrationModel.Email = UserParameter.Email;
                RegistrationModel.FactoryStaffEntityId = UserParameter.FactoryStaffEntityId;
            }
        }

        private async Task LoadRoles()
        {
            Roles = await usuarioService.GetRoles();
        }
        private async Task LoadEmployeeData()
        {
            Employees = await empleadoService.GetAll();
        }
        private async Task RegisterUser()
        {
            var response = await usuarioService.Register(RegistrationModel, RegistrationModel.Pwd!);

            if (response is not null)
            {
                await SweetAlertHelper.ShowSuccessAlert("Completado", "Usuario registrado exitosamente");
            }
            else
            {
                await SweetAlertHelper.ShowErrorAlert("Error", "No se pudo registrar el usuario");
            }
        }
        private async Task UpdateUser()
        {
            try
            {
                var user = new UserDTO
                {
                    Id = UserParameter!.Id,
                    Email = RegistrationModel.Email!,
                    Role = RegistrationModel.Role!,
                };

                var response = await usuarioService.UpdateUser(user!, RegistrationModel.Pwd!);

                if (response is not null)
                {
                    await SweetAlertHelper.ShowSuccessAlert("Completado", "Usuario actualizado exitosamente");
                }
                else
                {
                    await SweetAlertHelper.ShowErrorAlert("Error", "No se pudo actualizar el usuario");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar el usuario: {ex.Message}");
            }
        }
        private async Task OnValidSubmit()
        {
            if (UserParameter is null)
            {
                await RegisterUser();
                DialogService.Close(RegistrationModel);
            }
            else
            {
                await UpdateUser();
                DialogService.Close(RegistrationModel);
            }
        }
    }
}
