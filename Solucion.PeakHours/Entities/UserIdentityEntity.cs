using Microsoft.AspNetCore.Identity;
using SolucionPeakHours.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolucionPeakHours.Entities
{
    public class UserIdentityEntity : IdentityUser
    {
        public int FactoryStaffEntityId { get; set; }
        public bool AllowEmergencyTime { get; set; }
        [NotMapped] 
        public string Role { get; set; } = string.Empty;
        public FactoryStaffEntity? FactoryStaffEntity { get; set; }
    }
}
