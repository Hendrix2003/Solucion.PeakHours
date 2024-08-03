using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using PeakHours_Blazor.Helpers;
using PeakHours_Blazor.Models;
using PeakHours_Blazor.Pages.CompletedHour;
using PeakHours_Blazor.Pages.Shared;
using PeakHours_Blazor.Services;
using Radzen;
using SolucionPeakHours.Shared.CompletedHours;

namespace PeakHours_Blazor.Pages.CompletedHour
{
    [Authorize(Roles = "Administrator, GerenteArea, Supervisor")]
    public partial class Index
    {
        [Inject] DialogService? DialogService { get; set; }

        [Inject] BeamAuthenticationStateProviderHelper? BeamAuthenticationStateProviderHelper { get; set; }

        List<WorkHourDTO> WorkHours = new List<WorkHourDTO>();

        string pagingSummaryFormat = "Mostrando la página {0} de {1} <b>(total de {2} registros)</b>";

        IEnumerable<int> pageSizeOptions = Enumerable.Range(1, 300);
        bool showPagerSummary = true;

        private bool IsAreaManager = false;
        private bool IsAdmin = false;
        private bool IsSupervisor = false;
        private bool HasPermission = false;
        private bool CanApprove = false;
        private bool CanAcceptRequest = true;

        protected override async Task OnInitializedAsync()
        {
            await EvaluateRole();

            await LoadData();
        }

        private async Task EvaluateRole()
        {
            var authState = await BeamAuthenticationStateProviderHelper!.GetAuthenticationStateAsync();

            IsAreaManager = authState.User.IsInRole("GerenteArea");
            IsAdmin = authState.User.IsInRole("Administrator");
            IsSupervisor = authState.User.IsInRole("Supervisor");
            HasPermission = IsAreaManager || IsAdmin || IsSupervisor;
            CanApprove = IsAreaManager || IsAdmin;
        }

        private async Task LoadData()
        {
            try
            {
                var loadedWorkHours = await WorkHourService.GetWorkHoursByUserArea();

                WorkHours = loadedWorkHours.OrderBy(ph => ph.CreatedAt).ToList();

                WorkHours.Reverse();
            }
            catch (Exception)
            {
                WorkHours = new List<WorkHourDTO>();
            }
        }


        DateTime startDateFilter;
        DateTime endDateFilter;
        private async Task FilterData()
        {
            if (startDateFilter != default && endDateFilter != default)
            {
                WorkHours = (await WorkHourService.GetWorkHoursByUserArea())
                    .Where(ph => ph.FechaOld.Date >= startDateFilter.Date && ph.FechaOld.Date <= endDateFilter.Date)
                    .ToList();
            }
        }

        private async Task ApproveByAreaManager(int id)
        {
            var resultado = await SweetAlertHelper.ShowWarningAlert("Cambiar estado", "¿Deseas cambiar el estado de esta solicitud?");

            if (resultado)
            {
                await WorkHourService.ApproveByAreaManager(id);
                await LoadData();
            }
        }
        private async Task ApproveByFactoryManager(int id)
        {
            var resultado = await SweetAlertHelper.ShowWarningAlert("Cambiar estado", "¿Deseas cambiar el estado de esta solicitud?");

            if (resultado)
            {
                await WorkHourService.ApproveByFactoryManager(id);
                await LoadData();
            }
        }
        private async Task ApproveRequestByUserRole(WorkHourDTO workHour)
        {
            await EvaluateCanAcceptRequest(workHour);

            if (CanAcceptRequest)
            {
                if (IsAreaManager)
                    await ApproveByAreaManager(workHour.Id);
                else if (IsAdmin)
                    await ApproveByFactoryManager(workHour.Id);
            }
        }

        private async Task DeleteWorkHour(int id)
        {
            var resultado = await SweetAlertHelper.ShowWarningAlert("Eliminar Empleado", "¿Deseas eliminar el empleado?");

            if (resultado)
            {
                await WorkHourService.DeleteAsync(id);
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
        private async Task OpenFormDialog(string title, WorkHourDTO? workHourDTO = default)
        {
            var options = new DialogOptions() { Width = "auto", Height = "auto" };

            await DialogService
                .OpenAsync<CompletedHourForm>(title
                , parameters: new Dictionary<string, object>() { { "WorkHourDTOParameter", workHourDTO! } }
                , options);

            await LoadData();
        }

        private async Task EvaluateCanAcceptRequest(WorkHourDTO workHour)
        {
            if (IsAdmin)
            {
                if (!workHour.AreaManagerApproved)
                {
                    CanAcceptRequest = false;
                    await SweetAlertHelper.ShowErrorAlert("Aviso", "El gerente de área no ha aprobado esta solicitud.");
                }
            }
        }
        private SetStatus SetStatusRequest(WorkHourDTO workHour)
        {
            var response = new SetStatus();

            if (IsAreaManager)
            {
                response.Text = workHour.AreaManagerApproved ? "Aprobado" : "En espera";
                response.ButtonStyle = workHour.AreaManagerApproved ? ButtonStyle.Success : ButtonStyle.Danger;
            }

            if (IsAdmin)
            {
                response.Text = workHour.FactoryManagerApproved ? "Cambiar Estado" : "Aprobar";
                response.ButtonStyle = workHour.FactoryManagerApproved ? ButtonStyle.Info : ButtonStyle.Success;

                if(!workHour.AreaManagerApproved)
                {
                    response.ButtonStyle = ButtonStyle.Danger;
                    response.Text = "En espera";
                }
            }

            return response;
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
                hoja.Cell(1, 8).Value = "Tipo de Hora";
                hoja.Cell(1, 9).Value = "Cantidad de horas";
                hoja.Cell(1, 10).Value = "Total de horas";
                hoja.Cell(1, 11).Value = "Motivos";
                hoja.Cell(1, 12).Value = "Fecha programada";
                hoja.Cell(1, 13).Value = "Aprovado por G.A";
                hoja.Cell(1, 14).Value = "Aprovado por G.F";

                // Obtener datos ordenados de la cuadrícula
                var sortedData = WorkHours.OrderBy(item => item.ProgHourEntity!.FactoryStaffEntity!.Id);

                for (int fila = 1; fila <= sortedData.Count(); fila++)
                {
                    var currentData = sortedData.ElementAt(fila - 1);

                    hoja.Cell(fila + 1, 1).Value = currentData.ProgHourEntity!.FactoryStaffEntity!.Id;
                    hoja.Cell(fila + 1, 2).Value = currentData.ProgHourEntity!.FactoryStaffEntity!.FullName;
                    hoja.Cell(fila + 1, 3).Value = currentData.ProgHourEntity!.FactoryStaffEntity!.Manager;
                    hoja.Cell(fila + 1, 4).Value = currentData!.ProgHourEntity!.FactoryStaffEntity!.Area;
                    hoja.Cell(fila + 1, 5).Value = currentData!.ProgHourEntity!.FactoryStaffEntity!.SubArea;
                    hoja.Cell(fila + 1, 6).Value = currentData!.ProgHourEntity!.FactoryStaffEntity!.Position;
                    hoja.Cell(fila + 1, 7).Value = currentData!.ProgHourEntity!.FactoryStaffEntity!.Line;
                    hoja.Cell(fila + 1, 8).Value = currentData.HourType;
                    hoja.Cell(fila + 1, 9).Value = currentData.HoursWorked;
                    hoja.Cell(fila + 1, 11).Value = currentData.ReasonsRelize;
                    hoja.Cell(fila + 1, 12).Value = currentData.FechaOld;
                    hoja.Cell(fila + 1, 13).Value = currentData.AreaManagerApproved;
                    hoja.Cell(fila + 1, 14).Value = currentData.FactoryManagerApproved; 
                }

                using (var memoria = new MemoryStream())
                {
                    libro.SaveAs(memoria);
                    var nameExcel = string.Concat("Horas Extras Aprobadas ", DateTime.Now.ToString(), ".xlsx");

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

