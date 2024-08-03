using SolucionPeakHours.Shared.CompletedHours;
using SolucionPeakHours.Shared.ProgHours;

namespace SolucionPeakHours.Shared.Dashboard
{
    public class EmployeeDashboard
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? Manager { get; set; }
        public string? Area { get; set; }
        public string? SubArea { get; set; }
        public string? Position { get; set; }
        public string? Line { get; set; }
        public int? TotalHoursPerMonth { get; set; }

        public List<ProgHourDTO> ProgHours { get; set; }
        public List<WorkHourDTO> WorkHours { get; set; }
    }
}
