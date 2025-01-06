namespace Pharaonia.Domain.Interfaces.helpers
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
    }
}
