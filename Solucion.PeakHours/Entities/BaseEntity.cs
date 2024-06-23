using System.Diagnostics.CodeAnalysis;

namespace SolucionPeakHours.Models
{
    public class BaseEntity
    {
        [AllowNull] 
        public int? Id { get; set; }
        [AllowNull] public string CreatedByUserId { get; set; }
    }
}
