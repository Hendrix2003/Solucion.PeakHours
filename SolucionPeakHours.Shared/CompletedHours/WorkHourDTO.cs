
using SolucionPeakHours.Shared.Common;
using SolucionPeakHours.Shared.ProgHours;
using System.ComponentModel.DataAnnotations;

namespace SolucionPeakHours.Shared.CompletedHours
{
    public class WorkHourDTO : BaseDto
    {
        [Required(ErrorMessage = "EL campo{0} es requerido.")]
        public int? HoursWorked { get; set; }
        public string? HourType { get; set; }
        public string? ReasonsRelize { get; set; }
        public DateTime FechaOld { get; set; }
        public DateTime? CreatedAt { get; set; }
        public bool AreaManagerApproved { get; set; } = false;
        public bool FactoryManagerApproved { get; set; } = false;
        public int? ProgHourEntityId { get; set; }
        public virtual ProgHourDTO? ProgHourEntity { get; set; }
    }
}
