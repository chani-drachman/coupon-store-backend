using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CouponShop.BLL.Infrastructures
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            if (string.IsNullOrWhiteSpace(to)) throw new ArgumentException("to is required");
            if (string.IsNullOrWhiteSpace(subject)) subject = "(no subject)";

            var emailSettings = _config.GetSection("Email");
            string from = emailSettings["From"] ?? throw new InvalidOperationException("Email:From not configured");
            string smtpServer = emailSettings["SmtpServer"] ?? throw new InvalidOperationException("Email:SmtpServer not configured");

            if (!int.TryParse(emailSettings["Port"], out var port)) port = 587;
            string username = emailSettings["Username"];
            string password = emailSettings["Password"];
            bool enableSsl = bool.TryParse(emailSettings["EnableSsl"], out var ssl) && ssl;

            using var message = new MailMessage();
            message.From = new MailAddress(from);
            message.To.Add(new MailAddress(to));
            message.Subject = subject;
            message.Body = body ?? string.Empty;
            message.IsBodyHtml = true;

            try
            {
                using var client = new SmtpClient(smtpServer, port)
                {
                    Credentials = new NetworkCredential(username, password),
                    EnableSsl = enableSsl,
                };

                await client.SendMailAsync(message);
            }
            catch (SmtpException ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}
