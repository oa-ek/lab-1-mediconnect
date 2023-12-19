using System.Net;
using System.Net.Mail;

namespace Client.Services.EmailSender;

public class EmailSender : IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string message)
    {
        // налаштований email
        var mail = "yaroslav.borodii@oa.edu.ua";
        // app password
        var pw = "lkpx jhgu vbrk rnxv";

        var client = new SmtpClient("smtp.gmail.com", 587)
        {
            EnableSsl = true,
            Credentials = new NetworkCredential(mail, pw)
        };

        return client.SendMailAsync(mail, email, subject, message);
    }
}