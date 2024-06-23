using SolucionPeakHours.Shared.Common;
using SolucionPeakHours.Shared.FactoryStaff;
using System.ComponentModel.DataAnnotations;


namespace SolucionPeakHours.Shared.ProgHours
{
    public class ProgHourDTO : BaseDto
    {

        [Required(ErrorMessage = "EL campo{0} es requerido.")]
        public int? HourCant { get; set; }
        public string? Reasons { get; set; }
        public DateTime ProgDate { get; set; } = DateTime.Now;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string ProgHourShow
        {
            get
            {
                return $"{ProgDate:dd/MM/yyyy} - {HourCant} horas";
            }
        }

        public int? FactoryStaffEntityId { get; set; }
        public virtual FactoryStaffDTO? FactoryStaffEntity { get; set; }
    }
}
