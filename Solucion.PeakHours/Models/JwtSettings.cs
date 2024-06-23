namespace SolucionPeakHours.Models
{
    public class JwtSettings
    {
        public string Key { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public double ExpirationInMinutes { get; set; }

        public TimeSpan ExpireTime { get; set; }
    }
}
