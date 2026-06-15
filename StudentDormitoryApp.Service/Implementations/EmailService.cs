using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;
using StudentDormitoryApp.Domain.Email;
using StudentDormitoryApp.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;

namespace StudentDormitoryApp.Service.Implementations
{
    public class EmailService : IEmailService
    {

        private readonly IConfiguration _configuration;
        
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(EmailMessage message)
        {
            var smtpSettings = _configuration.GetSection("SmtpSettings");

            var email = new MimeMessage();

            email.From.Add(new MailboxAddress(
                smtpSettings["EmailDisplayName"],
                smtpSettings["SmtpUsername"]
            ));

            email.To.Add(MailboxAddress.Parse(message.MailTo));
            email.Subject = message.Subject;

            var builder = new BodyBuilder
            {
                HtmlBody = message.Content
            };

           
            if (message.Attachment != null)
            {
                builder.Attachments.Add(
                    message.AttachmentName ?? "file.pdf",
                    message.Attachment
                );
            }

            email.Body = builder.ToMessageBody();

            using var client = new MailKit.Net.Smtp.SmtpClient();

            await client.ConnectAsync(
                smtpSettings["SmtpServer"],
                int.Parse(smtpSettings["SmtpServerPort"]),
                MailKit.Security.SecureSocketOptions.StartTls
            );

            await client.AuthenticateAsync(
                smtpSettings["SmtpUsername"],
                smtpSettings["SmtpPassword"]
            );

            await client.SendAsync(email);
            await client.DisconnectAsync(true);

        }
    }
    
}
