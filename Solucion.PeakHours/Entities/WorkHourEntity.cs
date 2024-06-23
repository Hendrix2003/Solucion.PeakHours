namespace SolucionPeakHours.Models;

public partial class WorkHourEntity : BaseEntity
{
    public int? HoursWorked { get; set; }
    public string? HourType { get; set; }
    public string? ReasonsRelize { get; set; }
    public DateTime FechaOld { get; set; }
    public DateTime? CreatedAt { get; set; }
    public bool AreaManagerApproved { get; set; } = false;
    public bool FactoryManagerApproved { get; set; } = false;
    public int ProgHourEntityId { get; set; }
    public virtual ProgHourEntity? ProgHourEntity { get; set; }
}
