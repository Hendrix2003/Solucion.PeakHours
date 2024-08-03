using SolucionPeakHours.Shared.UserAccount;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace PeakHours_Blazor.Services
{
    public class EmailService : IEmailService
    {
        private readonly HttpClient _httpClient;

        public EmailService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task SendEmailAsync(EmailDTO request)
        {
            await _httpClient.PostAsJsonAsync("api/Email", request);
        }
    }
}
