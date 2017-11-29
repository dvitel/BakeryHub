using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace BakeryHub.Services
{
    public class EmailService : IEmailService
    {
        public Task SendAsync(string email, string subject, string text)
        {
            SmtpClient client = new SmtpClient("mysmtpserver");
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("?", "?");

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("notifier@bakeryHub.com");
            mailMessage.To.Add(email);
            mailMessage.Body = text;
            mailMessage.Subject = subject;
            client.SendAsync(mailMessage, null);
            return Task.CompletedTask;
        }
    }
}
