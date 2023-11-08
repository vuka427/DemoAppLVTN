using Application.DTOs.EmailMessage;
using Domain.Common;
using Domain.Entities;
using System;
using System.Collections.Generic;

using System.Text;
using System.Threading.Tasks;

namespace Application.Interface.ApplicationServices
{
    public interface IEmailService
    {
      
        Task<AppResult> SendMailCreateInvoice(string email, string receiverName, Contract contract, Invoice invoice);
        Task<AppResult> SendMailPayInvoice(string email, string receiverName, Contract contract, Invoice invoice);


    }
}
