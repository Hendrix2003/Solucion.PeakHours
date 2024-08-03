using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using PeakHours_Blazor.Interfaces;
using Radzen.Blazor;
using SolucionPeakHours.Shared.FactoryStaff;

namespace PeakHours_Blazor.Pages.Employee
{
    [Authorize(Roles = "Administrator")]
    public partial class EmployeeForm
    {
        [Inject] public IEmployeeService empleadoService { get; set; }
        [Inject] public SweetAlertService Swal { get; set; }
        [Inject] public NavigationManager navigationManager { get; set; }
        [Inject] public RadzenRequiredValidator RequiredValidator { get; set; }

        [Parameter] public FactoryStaffDTO FactoryStaffDTOParameter { get; set; }

        string btnText = string.Empty;
        private bool factoryStaffIdFieldDisable = false;
        private int? FactoryStaffId;
        FactoryStaffDTO FactoryStaffModel = new FactoryStaffDTO();

        protected override async Task OnInitializedAsync()
        {
            if (FactoryStaffDTOParameter is null)
            {
                btnText = "Guardar Empleado";
                return;
            }

            FactoryStaffModel = FactoryStaffDTOParameter;
            FactoryStaffId = FactoryStaffModel.Id;
            factoryStaffIdFieldDisable = true;
            btnText = "Actualizar Empleado";
        }

        private async Task Add()
        {
            await empleadoService.CreateAsync(FactoryStaffModel);
        }

        private async Task Update()
        {

            await empleadoService.UpdateAsync(FactoryStaffModel);
        }

        private async Task OnValidSubmit()
        {
            if (FactoryStaffDTOParameter is null)
            {
                await Add();
            }
            else
            {
                await Update();
            }
            navigationManager.NavigateTo("/employee/index");
        }
    }
}
