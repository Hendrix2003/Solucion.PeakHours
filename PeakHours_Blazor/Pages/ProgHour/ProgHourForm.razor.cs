using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using PeakHours_Blazor.Interfaces;
using Radzen;
using Radzen.Blazor;
using SolucionPeakHours.Shared.FactoryStaff;
using SolucionPeakHours.Shared.ProgHours;

namespace PeakHours_Blazor.Pages.ProgHour
{
    [Authorize(Roles = "Administrator, GerenteArea, Supervisor")]
    public partial class ProgHourForm
    {
        [Inject] public IProgHourService? supervisorService { get; set; }
        [Inject] public IEmployeeService? employeeService { get; set; }
        [Inject] public SweetAlertService? Swal { get; set; }
        [Inject] public NavigationManager? navigationManager { get; set; }
        [Inject] public RadzenRequiredValidator? RequiredValidator { get; set; }
        [Inject] public DialogService? DialogService { get; set; }

        [Parameter] public ProgHourDTO? ProgHourDTOParameter { get; set; }

        string btnText = string.Empty;
        private bool factoryStaffIdFieldDisable = false;
        private int? FactoryStaffId;
        ProgHourDTO ProhHourModel = new ProgHourDTO();
        FactoryStaffDTO FactoryStaffModel = new FactoryStaffDTO();

        protected override async Task OnInitializedAsync()
        {
            if (ProgHourDTOParameter is null)
            {
                ProhHourModel.CreatedAt = DateTime.Today;
                btnText = "Guardar Empleado";
                return;
            }

            await LoadFactoryStaffById((int)ProgHourDTOParameter.FactoryStaffEntityId!);
            ProhHourModel = ProgHourDTOParameter;
            FactoryStaffId = FactoryStaffModel.Id;
            factoryStaffIdFieldDisable = true;
            btnText = "Actualizar Empleado";
        }

        private async Task SearchFactoryStaffWithKeyUp()
        {
            if (FactoryStaffId != null)
            {
                try
                {
                    var factoryStaff = await employeeService.GetByIdAsync((int)FactoryStaffId!);

                    FactoryStaffModel = factoryStaff;

                    StateHasChanged();
                }
                catch (Exception)
                {
                    FactoryStaffModel = new FactoryStaffDTO();
                }
            }
            else
            {
                FactoryStaffModel = new FactoryStaffDTO();
            }
        }

        private async Task LoadFactoryStaffById(int id)
        {
            try
            {
                var factoryStaff = await employeeService!.GetByIdAsync(id);

                FactoryStaffModel = factoryStaff;

                StateHasChanged();
            }
            catch (Exception)
            {
                FactoryStaffModel = new FactoryStaffDTO();
            }
        }

        private async Task Add()
        {
            ProhHourModel.FactoryStaffEntityId = FactoryStaffModel.Id;

            await supervisorService!.CreateAsync(ProhHourModel);
        }

        private async Task Update()
        {
            ProhHourModel.FactoryStaffEntityId = FactoryStaffModel.Id;

            await supervisorService!.UpdateAsync(ProhHourModel);
        }

        private async Task OnValidSubmit()
        {
            if (ProgHourDTOParameter is null)
            {
                await Add();
            }
            else
            {
                await Update();
            }

            DialogService!.Close();
        }
    }
}
