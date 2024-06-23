namespace SolucionPeakHours.Shared.UserAccount
{
    public class UserStateAuthResponse
    {
        public bool IsAuthenticated { get; set; }
        public string Id { get; set; }
        public string Role { get; set; }
        public string FullName { get; set; }
        public string Area { get; set; }
    }
}
