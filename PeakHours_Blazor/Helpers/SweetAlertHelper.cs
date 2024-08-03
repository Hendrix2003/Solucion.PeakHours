using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.JSInterop;

namespace PeakHours_Blazor.Helpers
{
    public class SweetAlertHelper
    {
        private static SweetAlertService Swal;

        public static void Initialize(IJSRuntime jsRuntime)
        {
            Swal = new SweetAlertService(jsRuntime);
        }

        public static async Task ShowInfoAlert(string title, string message)
        {
            await Swal.FireAsync(new SweetAlertOptions
            {
                Title = title,
                Text = message,
                Icon = SweetAlertIcon.Info
            });
        }

        public static async Task ShowToastAlert(string title, string message, SweetAlertIcon? sweetAlertIcon = null)
        {
            await Swal.FireAsync(new SweetAlertOptions
            {
                Title = title,
                Text = message,
                Icon = sweetAlertIcon ?? SweetAlertIcon.Success,
                Toast = true,
                Position = SweetAlertPosition.TopEnd,
                ShowConfirmButton = false,
                Timer = 2000
            });
        }

        public static async Task ShowSuccessAlert(string title, string message)
        {
            await Swal.FireAsync(new SweetAlertOptions
            {
                Title = title,
                Text = message,
                Icon = SweetAlertIcon.Success
            });
        }

        public static async Task ShowErrorAlert(string title, string message)
        {
            await Swal.FireAsync(new SweetAlertOptions
            {
                Title = title,
                Text = message,
                Icon = SweetAlertIcon.Error
            });
        }

        public static async Task<bool> ShowWarningAlert(string title, string message)
        {
            var action = await Swal.FireAsync(new SweetAlertOptions
            {
                Title = title,
                Text = message,
                Icon = SweetAlertIcon.Warning,
                ShowCancelButton = true,
                ConfirmButtonText = "Aceptar",
                CancelButtonText = "Cancelar"
            });

            return !string.IsNullOrEmpty(action.Value);
        }
    }
}
