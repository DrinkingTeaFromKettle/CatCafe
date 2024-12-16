using Humanizer;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;

namespace CatCafe.Services
{
    public class EmailSender: IEmailSender
    {
        private readonly SmtpClient _smtpClient;
        public EmailSender() 
        {
            _smtpClient = new SmtpClient("sandbox.smtp.mailtrap.io", 2525)
            {
                Credentials = new NetworkCredential(),
                EnableSsl = true
            };
        }
        

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            MailMessage message = new MailMessage();
            message.Subject = subject;
            message.Body = htmlMessage;
            message.From = new MailAddress("CatCafe-noreply@example.com");
            message.To.Add( new MailAddress(email));
            message.IsBodyHtml = true;
            _smtpClient.SendAsync(message, email);
        }
    }
}
