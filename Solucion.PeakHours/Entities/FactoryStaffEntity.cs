namespace SolucionPeakHours.Models;

public partial class FactoryStaffEntity : BaseEntity
{
    public string? FullName { get; set; }
    public string? Manager { get; set; }
    public string? Area { get; set; }
    public string? SubArea { get; set; }
    public string? Position { get; set; }
    public string? Line { get; set; }
    public int? TotalHoursPerMonth { get; set; }

    public virtual ICollection<ProgHourEntity> ProgHours { get; set; }
}
