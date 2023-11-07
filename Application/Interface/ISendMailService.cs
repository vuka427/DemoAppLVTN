using Application.DTOs.EmailMessage;
using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface ISendMailService
    {

        Task<AppResult> SendMail(MailContent mailContent);

        Task<AppResult> SendEmailAsync(string email, string subject, string htmlMessage);


    }
}
