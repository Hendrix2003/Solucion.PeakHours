
namespace SolucionPeakHours.Models
{
    public partial class ProgHourEntity : BaseEntity
    {
        public int? HourCant { get; set; }
        public string? Reasons { get; set; }
        public DateTime ProgDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool ProgExpired { get; set; } = false;
        public int? FactoryStaffEntityId { get; set; }
        public virtual FactoryStaffEntity? FactoryStaffEntity { get; set; }
        public virtual ICollection<WorkHourEntity> WorkHours { get; set; }
    }
}