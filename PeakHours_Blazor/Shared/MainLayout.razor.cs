using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Radzen;
using SolucionPeakHours.Shared.UserAccount;

namespace PeakHours_Blazor.Shared
{
    [Authorize(Roles = "Administrator, GerenteArea, Supervisor")]
    public partial class MainLayout
    {
        List<EmailDTO> emails = new List<EmailDTO>();
        [Inject] DialogService? DialogService { get; set; }


        private bool IsAreaManager = false;
        private bool IsAdmin = false;
        private bool IsSupervisor = false;
        private bool HasPermission = false;
       
        private async Task LoadData()
        {

            emails = new List<EmailDTO>();
        }

        protected override async Task OnInitializedAsync()
        {
            await EvaluateRole();
            await LoadData();
            await beamAuthenticationStateProviderHelper!.GetAuthenticationStateAsync();
        }
        private async Task OpenFormDialogEmail(string title, EmailDTO? emailDTO = default)
        {
            var options = new DialogOptions() { Width = "auto", Height = "auto" };

            await DialogService!
                .OpenAsync<EmailForm>(title
                , parameters: new Dictionary<string, object>() { { "EmailTOParameter", emailDTO! } }
                , options);

            await LoadData();
        }

        private async Task EvaluateRole()
        {
            var authState = await BeamAuthenticationStateProviderHelper!.GetAuthenticationStateAsync();

            IsAreaManager = authState.User.IsInRole("GerenteArea");
            IsAdmin = authState.User.IsInRole("Administrator");
            IsSupervisor = authState.User.IsInRole("Supervisor");
            HasPermission = IsAreaManager || IsAdmin || IsSupervisor;
        }

    }
}
