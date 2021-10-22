using Email.Data.config;
using Email.Data.model;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Email.Data.service
{
    public class EmailService : IEmailService
    {

        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            this._emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(EmailInfo emailInfo)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_emailSettings.From));
            email.To.Add(MailboxAddress.Parse(emailInfo.EmailTo));
            email.Subject = emailInfo.Subject;

            var builder = new BodyBuilder();
            if (emailInfo.Attachments != null)
            {
                byte[] fileBytes;

                foreach(var file in emailInfo.Attachments)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            fileBytes = ms.ToArray();
                        }
                        builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                    }
                }
            }

            builder.HtmlBody = emailInfo.Body;
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            smtp.Connect(_emailSettings.SmtpServer, _emailSettings.Port, SecureSocketOptions.StartTls);
            smtp.AuthenticationMechanisms.Remove("XOAUTH2");
            smtp.Authenticate(_emailSettings.Username, _emailSettings.Password);

            await smtp.SendAsync(email);
            smtp.Disconnect(true);
            smtp.Dispose();
        }

        //public async Task SendEmailTemplateAsync(EmailSource emailSource)
        //{

        //}
    }
}
