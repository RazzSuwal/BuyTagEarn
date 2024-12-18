
using SMM.DataAccessLayer.Services.IServices;
using System.Net;
using System.Net.Mail;

namespace SMM.DataAccessLayer.Services.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var mail = "you-gmail";
            var appPassword = "your key";

            var client = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, appPassword)
            };

            return client.SendMailAsync(
                new MailMessage(from: mail, to: email, subject: subject, body: htmlMessage)
            );
        }
    }
}
