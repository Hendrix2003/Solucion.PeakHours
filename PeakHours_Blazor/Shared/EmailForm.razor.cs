using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using PeakHours_Blazor.Interfaces;
using Radzen;
using Radzen.Blazor;
using SolucionPeakHours.Shared.UserAccount;

namespace PeakHours_Blazor.Shared
{
    [Authorize(Roles = "Administrator, GerenteArea, Supervisor")]
    public partial class EmailForm
    {
        [Inject] public IWorkHourService? workHourService { get; set; }
        [Inject] public IUserService? usuarioService { get; set; }
        [Inject] public IProgHourService? progHourService { get; set; }
        [Inject] public IEmailService? EmailService { get; set; }
        [Inject] public DialogService? DialogService { get; set; }
        [Inject] public SweetAlertService? SweetAlertService { get; set; }

        [Parameter] public EmailDTO? EmailTOParameter { get; set; }

        private RegisterDTO RegistrationModel = new RegisterDTO();
        private List<UserDTO> Users { get; set; } = new List<UserDTO>();

        string btnText = string.Empty;
        EmailDTO EmailModel = new EmailDTO();

        protected override async Task OnInitializedAsync()
        {
            await LoadUsers();

            if (EmailTOParameter != null)
            {
                btnText = "Enviar Correo";
                EmailModel = EmailTOParameter;
            }
        }
        private async Task LoadUsers()
        {
            Users = await usuarioService!.GetUsersAsync();
        }
        private async Task OnValidSubmit()
        {
            await EmailService!.SendEmailAsync(EmailModel);
            await SweetAlertService!.FireAsync(new SweetAlertOptions
            {
                Title = "Correo Enviado",
                Icon = SweetAlertIcon.Success,
            });

            DialogService!.Close();
        }

        private void OnUserSelected(dynamic value)
        {
            EmailModel.To = value;
        }
    }

}
