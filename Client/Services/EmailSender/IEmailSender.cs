namespace Client.Services.EmailSender;

public interface IEmailSender
{
    Task SendEmailAsync(string email, string subject, string message);
}