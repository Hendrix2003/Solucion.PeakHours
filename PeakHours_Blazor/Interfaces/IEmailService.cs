using SolucionPeakHours.Shared.UserAccount;

public interface IEmailService
{
    Task SendEmailAsync(EmailDTO request);
}