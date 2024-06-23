
using SolucionPeakHours.Shared.FactoryStaff;

namespace SolucionPeakHours.Shared.UserAccount
{
    public class UserDTO
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public int FactoryStaffEntityId { get; set; }
        public bool AllowEmergencyTime { get; set; }
        public FactoryStaffDTO? FactoryStaffEntity { get; set; }
    }
}
