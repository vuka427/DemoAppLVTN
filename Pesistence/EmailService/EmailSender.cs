using Application.DTOs.EmailMessage;
using Application.Interface;
using Domain.Common;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using Org.BouncyCastle.Asn1.Smime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Pesistence.EmailService
{
    public class MailSettings
    {
        public string Mail { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }

    }



    public class EmailSender : ISendMailService
    {
        private readonly MailSettings mailSettings;

        public EmailSender(IOptions<MailSettings>  mailSettings)
        {
            this.mailSettings=mailSettings.Value;
        }

        public async Task<AppResult> SendMail(MailContent mailContent)
        {
            var email = new MimeMessage();

            email.Sender = new MailboxAddress(mailSettings.DisplayName, mailSettings.Mail);
            email.From.Add(new MailboxAddress(mailSettings.DisplayName, mailSettings.Mail));
            email.To.Add(MailboxAddress.Parse(mailContent.To));
            email.Subject = mailContent.Subject;


            var builder = new BodyBuilder();
            builder.HtmlBody = mailContent.Body;
            email.Body = builder.ToMessageBody();

            // dùng SmtpClient của MailKit
            using var smtp = new MailKit.Net.Smtp.SmtpClient();

            try
            {
                smtp.Connect(mailSettings.Host, mailSettings.Port, SecureSocketOptions.StartTls);
              
                smtp.Authenticate(mailSettings.Mail, mailSettings.Password);

                var rs = await smtp.SendAsync(email);

                smtp.Disconnect(true); 

                return new AppResult { Success = true, Message="Gửi mail thành công!" };
            } 
            catch (Exception ex)
            {
                // Gửi mail thất bại, nội dung email sẽ lưu vào thư mục mailssave
                System.IO.Directory.CreateDirectory("mailssave");
                var emailsavefile = string.Format(@"mailssave/{0}.eml", Guid.NewGuid());
                await email.WriteToAsync(emailsavefile);

                smtp.Disconnect(true);
                return new AppResult { Success = false, Message="Gửi mail thất bại!" };

            }
        }

        public async Task<AppResult> SendEmailAsync(string email, string subject, string htmlMessage)
        {
           return await SendMail(new MailContent()
            {
                To = email,
                Subject = subject,
                Body = htmlMessage
            });
        }

        
    }
}
