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
            email.From.Add(new MailboxAddress(smtpSettings["EmailDisplayName"], smtpSettings["SmtpUsername"]));
            email.To.Add(MailboxAddress.Parse(message.MailTo));
            email.Subject = message.Subject;
            email.Body = new TextPart("html") { Text = message.Content };

            using var client = new MailKit.Net.Smtp.SmtpClient();
            await client.ConnectAsync(smtpSettings["SmtpServer"], int.Parse(smtpSettings["SmtpServerPort"]), MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(smtpSettings["SmtpUsername"], smtpSettings["SmtpPassword"]);
            await client.SendAsync(email);
            await client.DisconnectAsync(true);

            //var smtpServer = smtpSettings["SmtpServer"];
            //var smtpPort = int.Parse(smtpSettings["SmtpServerPort"]);
            //var emailDisplayName = smtpSettings["EmailDisplayName"];
            //var senderName = smtpSettings["SenderName"];
            //var smtpUsername = smtpSettings["SmtpUsername"];
            //var smtpPassword = smtpSettings["SmtpPassword"];


            //using (var client = new SmtpClient(smtpServer, smtpPort))
            //{
            //    client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
            //    client.EnableSsl = true;

            //    var messageToSend = new MailMessage
            //    {
            //        From = new MailAddress(smtpUsername, emailDisplayName),
            //        Subject = message.Subject,
            //        Body = message.Content,
            //        IsBodyHtml = true
            //    };

            //    messageToSend.To.Add(message.MailTo);

            //    await client.SendMailAsync(messageToSend);

            //}

        }
    }
    
}
