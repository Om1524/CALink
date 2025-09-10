using CALink.Application.DTOs.Common_Dto;
using CALink.Application.Interfaces.Common_Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CALink.Application.Services.Common_Service
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;       

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendAsync(EmailDto message)
        {
            var smtpHost = _configuration["Email:Smtp:Host"];
            var smtpPort = int.Parse(_configuration["Email:Smtp:Port"]);
            var smtpUser = _configuration["Email:Smtp:Username"];
            var smtpPass = _configuration["Email:Smtp:Password"];
            var from = _configuration["Email:Smtp:From"];

            using var client = new SmtpClient(smtpHost, smtpPort)
            {
                Credentials = new NetworkCredential(smtpUser, smtpPass),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(from!),
                Subject = message.Subject,
                Body = message.Body,
                IsBodyHtml = message.IsBodyHtml
            };

            mailMessage.To.Add(message.To!);

            // ✅ Set Reply-To if provided
            if (!string.IsNullOrWhiteSpace(message.ReplyTo))
            {
                mailMessage.ReplyToList.Add(new MailAddress(message.ReplyTo));
            }

            await client.SendMailAsync(mailMessage);
        }
    }
}
