using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using PeakHours_Blazor.Helpers;
using PeakHours_Blazor.Interfaces;
using PeakHours_Blazor.Models;
using Radzen;
using Radzen.Blazor;
using SolucionPeakHours.Shared.CompletedHours;
using SolucionPeakHours.Shared.FactoryStaff;
using SolucionPeakHours.Shared.ProgHours;

namespace PeakHours_Blazor.Pages.CompletedHour
{
    [Authorize(Roles = "Administrator, GerenteArea, Supervisor")]
    public partial class CompletedHourForm
    {
        [Inject] public IWorkHourService? workHourService { get; set; }
        [Inject] public IProgHourService? progHourService { get; set; }
        [Inject] public IEmployeeService? employeeService { get; set; }
        [Inject] public SweetAlertService? Swal { get; set; }
        [Inject] public NavigationManager? navigationManager { get; set; }
        [Inject] public RadzenRequiredValidator? RequiredValidator { get; set; }
        [Inject] public DialogService? DialogService { get; set; }

        [Parameter] public WorkHourDTO? WorkHourDTOParameter { get; set; }

        [Parameter]
        public string hourtype { get; set; }

        string btnText = string.Empty;
        private bool factoryStaffIdFieldDisable = false;
        private int? FactoryStaffId;
        WorkHourDTO WorkHourModel = new WorkHourDTO();
        FactoryStaffDTO FactoryStaffModel = new FactoryStaffDTO();

        private HourTypeFilter HourFilter = new HourTypeFilter();

        List<DropDownData> HourType = new List<DropDownData>();


        List<ProgHourDTO> ProgHours = new List<ProgHourDTO>();

        private void LoadDropDownData()
        {
            HourType = new List<DropDownData>();
            HourType.Add(new DropDownData { Text = "Descanso (Rojas)" });
            HourType.Add(new DropDownData { Text = "Extras (Azules)" });
            HourType.Add(new DropDownData { Text = "Feriado" });
            
        }

        protected override void OnInitialized()
        {
            LoadDropDownData();

            HourFilter.HourTypeDay = hourtype;
        }
        protected override async Task OnInitializedAsync()
        {
            if (WorkHourDTOParameter is null)
            {
                WorkHourModel.CreatedAt = DateTime.Today;
                btnText = "Guardar Empleado";

                if(WorkHourDTOParameter!
                    .ProgHourEntity!
                    .FactoryStaffEntity!
                    .TotalHoursPerMonth > 28)
                {
                    await SweetAlertHelper.ShowInfoAlert("Atención", "El empleado está apunto de sobrepasar las 28 horas extras en el mes. Tener precaución.");
                }

                return;
            }

            await LoadFactoryStaffById((int)WorkHourDTOParameter
                .ProgHourEntity!
                .FactoryStaffEntityId!);

            var progHoursEmployee = await progHourService!.GetProgHoursByEmployeeId((int)WorkHourDTOParameter
                                                                                   .ProgHourEntity
                                                                                   .FactoryStaffEntityId);

            ProgHours = progHoursEmployee;
            WorkHourModel = WorkHourDTOParameter;
            FactoryStaffId = FactoryStaffModel.Id;
            factoryStaffIdFieldDisable = true;
            btnText = "Actualizar Empleado";
        }

        private void SetValueOnDropDownChange()
        {
            WorkHourModel.HoursWorked = ProgHours
                .Where(x => x.Id == WorkHourModel.ProgHourEntityId)
                .FirstOrDefault()!
                .HourCant;

            WorkHourModel.FechaOld = ProgHours
                .Where(x => x.Id == WorkHourModel.ProgHourEntityId)
                .FirstOrDefault()!
                .ProgDate;

            StateHasChanged();
        }

        private async Task SearchFactoryStaffWithKeyUp()
        {
            if (FactoryStaffId != null)
            {
                try
                {
                    var factoryStaff = await employeeService!.GetByIdAsync((int)FactoryStaffId!);

                    var progHoursEmployee = await progHourService!.GetProgHoursByEmployeeId((int)FactoryStaffId!);

                    ProgHours = progHoursEmployee;

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
            await workHourService!.CreateAsync(WorkHourModel);
        }

        private async Task Update()
        {
            await workHourService!.UpdateAsync(WorkHourModel);
        }

        private async Task OnValidSubmit()
        {
            if (WorkHourDTOParameter is null)
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

