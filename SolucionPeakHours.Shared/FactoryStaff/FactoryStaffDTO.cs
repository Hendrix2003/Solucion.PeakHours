using System.ComponentModel.DataAnnotations;

namespace SolucionPeakHours.Shared.FactoryStaff

{
    public class FactoryStaffDTO
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "EL campo{0} es requerido.")]

        public string? FullName { get; set; }

        public string? Manager { get; set; }

        public string? Area { get; set; }

        public int? TotalHoursPerMonth { get; set; }

        public string? SubArea { get; set; }

        public string? Position { get; set; }

        public string? Line { get; set; }

        public DateTime? CreatedAt { get; set; } = DateTime.Now;
    }
}
