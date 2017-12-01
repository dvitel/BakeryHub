using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace BakeryHub.Services
{
    public class GmailEmailService : IEmailService
    {
        IConfiguration config;
        public GmailEmailService(IConfiguration configu) => config = configu;
        public Task SendAsync(string email, string subject, string text)
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential(config["EmailCredentials:Login"], config["EmailCredentials:Password"]);

            MailMessage mailMessage = new MailMessage(config["EmailCredentials:Login"], email)
            {
                Subject = subject,
                Body = text
            };
            client.Send(mailMessage);
            return Task.CompletedTask;
        }
    }
}
