using ClosedXML.Excel;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using PeakHours_Blazor.Helpers;
using PeakHours_Blazor.Interfaces;
using PeakHours_Blazor.Pages.Shared;
using Radzen;
using SolucionPeakHours.Shared.ProgHours;

namespace PeakHours_Blazor.Pages.ProgHour
{
    [Authorize(Roles = "Administrator, GerenteArea, Supervisor")]
    public partial class Index
    {
        [Inject] DialogService? DialogService { get; set; }
        [Inject] IUserService? UserService { get; set; }
        [Inject] BeamAuthenticationStateProviderHelper BeamAuthenticationStateProviderHelper { get; set; }

        private bool IsAdmin { get; set; }
        private bool IsGerenteArea { get; set; }
        private bool IsSupervisor { get; set; }
        private bool IsInLast4DaysAdd  { get; set; }


        private bool HasPermission { get; set; }

        List<ProgHourDTO> ProgHours = new List<ProgHourDTO>();

        string pagingSummaryFormat = "Mostrando la página {0} de {1} <b>(total de {2} registros)</b>";

        IEnumerable<int> pageSizeOptions = Enumerable.Range(1, 300);

        bool showPagerSummary = true;
        bool allowEmergencyTime = false;

        private async Task EvaluateRole()
        {
            var authState = await BeamAuthenticationStateProviderHelper.GetAuthenticationStateAsync();

            IsAdmin = authState.User.IsInRole("Administrator");
            IsGerenteArea = authState.User.IsInRole("GerenteArea");
            IsSupervisor = authState.User.IsInRole("Supervisor");

            HasPermission = IsAdmin || IsGerenteArea || IsSupervisor;
        }

        protected override async Task OnInitializedAsync()
        {
            IsInLast4DaysAdd = IsInLast4DaysOfMonth();

            await EvaluateRole();

            await LoadData();

            await CheckEmergencyTimePermissionAsync();
        }

        private async Task CheckEmergencyTimePermissionAsync()
        {
            var userLogged = await UserService!.GetCurrentUserState();

            var user = await UserService!.GetUserById(userLogged.Id!);

            allowEmergencyTime = user!.AllowEmergencyTime;

            StateHasChanged();
        }

        private async Task LoadData()
        {
            try
            {
                var progHours = await supervisorService.GetProgHours();

                ProgHours = progHours;
            }
            catch (Exception)
            {
                ProgHours = new List<ProgHourDTO>();
            }
        }

        private  bool IsInLast4DaysOfMonth()
        {
            // Obtener la fecha actual
            DateTime fechaActual = DateTime.Now;

            // Obtener el último día del mes actual
            DateTime ultimoDiaDelMes = new DateTime(fechaActual.Year, fechaActual.Month, 1).AddMonths(1).AddDays(-1);

            // Obtener la fecha hace 4 días
            DateTime hace4Dias = fechaActual.AddDays(-4);

            // Verificar si la fecha actual está dentro de los últimos 4 días del mes
            return hace4Dias <= fechaActual && fechaActual <= ultimoDiaDelMes;
        }
        private async Task OnPage(PagerEventArgs args)
        {
        }

        DateTime startDateFilter;
        DateTime endDateFilter;

        private async Task FilterData()
        {
            if (startDateFilter != default && endDateFilter != default)
            {
                ProgHours = (await supervisorService.GetProgHours())
                    .Where(ph => ph.ProgDate.Date >= startDateFilter.Date && ph.ProgDate.Date <= endDateFilter.Date)
                    .ToList();
            }
        }

        private async Task DeleteProgHour(int id)
        {
            var resultado = await Swal.FireAsync(new SweetAlertOptions
            {
                Title = "Eliminar Empleado",
                Text = "¿Deseas eliminar el empleado?",
                Icon = SweetAlertIcon.Question,
                ShowCancelButton = true
            });

            if (resultado.IsConfirmed)
            {
                var idString = id.ToString();
                await supervisorService.DeleteAsync(id);
                await LoadData();
            }


        }
        private async Task OpenViewReason(string reason)
        {
            await DialogService!.OpenAsync<ReasonsView>(
                "Motivo"
                , new Dictionary<string, object>()
                {
                    { "Reason", reason }
                }
                , new DialogOptions
                {
                    Width = "auto",
                    Height = "auto"
                });
        }
        private async Task OpenFormDialog(string title, ProgHourDTO? progHourDTO = default)
        {
            var options = new DialogOptions() { Width = "auto", Height = "auto" };

            var dialog = await DialogService!
                .OpenAsync<ProgHourForm>(title
                , parameters: new Dictionary<string, object>() { { "ProgHourDTOParameter", progHourDTO! } }
                , options);

            await LoadData();
            await CheckEmergencyTimePermissionAsync();
        }
        private async Task ExportExcel(string buttonText)
        {
            using (var libro = new XLWorkbook())
            {
                IXLWorksheet hoja = libro.Worksheets.Add("empleados");

                hoja.Cell(1, 1).Value = "ID";
                hoja.Cell(1, 2).Value = "Nombre Empleado";
                hoja.Cell(1, 3).Value = "Gestor";
                hoja.Cell(1, 4).Value = "Area";
                hoja.Cell(1, 5).Value = "SubArea";
                hoja.Cell(1, 6).Value = "Posicion";
                hoja.Cell(1, 7).Value = "Linea";
                hoja.Cell(1, 8).Value = "Cantidad Hora";
                hoja.Cell(1, 9).Value = "Motivos";
                hoja.Cell(1, 10).Value = "Fecha Programada";


                for (int fila = 1; fila <= ProgHours.Count; fila++)
                {
                    hoja.Cell(fila + 1, 1).Value = ProgHours[fila - 1].FactoryStaffEntity!.Id;
                    hoja.Cell(fila + 1, 2).Value = ProgHours[fila - 1].FactoryStaffEntity!.FullName;
                    hoja.Cell(fila + 1, 3).Value = ProgHours[fila - 1].FactoryStaffEntity!.Manager;
                    hoja.Cell(fila + 1, 4).Value = ProgHours[fila - 1].FactoryStaffEntity!.Area;
                    hoja.Cell(fila + 1, 5).Value = ProgHours[fila - 1].FactoryStaffEntity!.SubArea;
                    hoja.Cell(fila + 1, 6).Value = ProgHours[fila - 1].FactoryStaffEntity!.Position;
                    hoja.Cell(fila + 1, 7).Value = ProgHours[fila - 1].FactoryStaffEntity!.Line;
                    hoja.Cell(fila + 1, 8).Value = ProgHours[fila - 1].HourCant;
                    hoja.Cell(fila + 1, 9).Value = ProgHours[fila - 1].Reasons;
                    hoja.Cell(fila + 1, 10).Value = ProgHours[fila - 1].ProgDate;


                }

                using (var memoria = new MemoryStream())
                {
                    libro.SaveAs(memoria);
                    var nameExcel = string.Concat("Reporte ", DateTime.Now.ToString(), ".xlsx");

                    await jsRuntime.InvokeAsync<object>(
                        "DownloadExcelFile",
                        nameExcel,
                        Convert.ToBase64String(memoria.ToArray())
                    );
                }
            }
        }
    }
}


