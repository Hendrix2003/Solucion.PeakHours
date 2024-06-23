

using SolucionPeakHours.Shared.UserAccount;

namespace SolucionPeakHours.Interfaces
{
    public interface IEmailService
    {
        void SendEmail(EmailDTO request);


    }
}
