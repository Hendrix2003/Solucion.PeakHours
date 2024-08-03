using ClosedXML.Excel;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using PeakHours_Blazor.Helpers;
using Radzen;
using SolucionPeakHours.Shared.FactoryStaff;

namespace PeakHours_Blazor.Pages.Employee
{
    [Authorize(Roles = "Administrator")]
    public partial class Index
    {
        [Inject] DialogService DialogService { get; set; }
        [Inject] BeamAuthenticationStateProviderHelper BeamAuthenticationStateProviderHelper { get; set; }

        List<FactoryStaffDTO> Employees = new List<FactoryStaffDTO>();

        private bool IsAdmin { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var authState = await BeamAuthenticationStateProviderHelper.GetAuthenticationStateAsync();

            IsAdmin = authState.User.IsInRole("Administrator");

            await LoadData();
        }
        private async Task LoadData()
        {
            try
            {
                Employees = await empleadoService.GetAll();
            }
            catch (Exception)
            {
                Employees = new List<FactoryStaffDTO>();
            }
        }
        private async Task DeleteEmployee(int id)
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
                await empleadoService.DeleteAsync(id);
                await LoadData();
            }
        }
        private async Task OpenFormDialog(string title, FactoryStaffDTO? factoryStaffDTO = default)
        {

            var options = new DialogOptions() { Width = "auto", Height = "auto" };

            var dialog = await DialogService
                .OpenAsync<EmployeeForm>(title
                , parameters: new Dictionary<string, object>() { { "FactoryStaffDTOParameter", factoryStaffDTO! } }
                , options);

            await LoadData();
        }
        private async Task ExportExcel()
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

                for (int fila = 1; fila <= Employees.Count; fila++)
                {
                    hoja.Cell(fila + 1, 1).Value = Employees[fila - 1].Id;
                    hoja.Cell(fila + 1, 2).Value = Employees[fila - 1].FullName;
                    hoja.Cell(fila + 1, 3).Value = Employees[fila - 1].Manager;
                    hoja.Cell(fila + 1, 4).Value = Employees[fila - 1].Area;
                    hoja.Cell(fila + 1, 5).Value = Employees[fila - 1].SubArea;
                    hoja.Cell(fila + 1, 6).Value = Employees[fila - 1].Position;
                    hoja.Cell(fila + 1, 7).Value = Employees[fila - 1].Line;
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
