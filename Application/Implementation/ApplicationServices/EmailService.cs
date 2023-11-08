using Application.Interface;
using Application.Interface.ApplicationServices;
using Application.Utility;
using Domain.Common;
using Domain.Entities;
using Domain.Interface;
using Domain.IRepositorys;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Implementation.ApplicationServices
{
    public class EmailService : IEmailService
    {


        private readonly IUnitOfWork _unitOfWork;
        private readonly ISendMailService _sendMailService;

        public EmailService(IUnitOfWork unitOfWork, ISendMailService sendMailService)
        {
            _unitOfWork=unitOfWork;
            _sendMailService=sendMailService;
        }

        public async Task<AppResult> SendMailCreateInvoice(string email, string receiverName,Contract contract, Invoice invoice)
        {
            var htmlMessage = GenerateInvoiceToHtmlMessage.ConverterCreateInvoiceToHtml(contract,invoice);



            var result =  _sendMailService.SendEmailAsync(email,"Thông báo tiền trọ", htmlMessage);

           


            return new AppResult {Success = true , Message="ok" };
        }

        public Task<AppResult> SendMailPayInvoice(string email, string receiverName, Invoice invoice)
        {
            throw new NotImplementedException();
        }
    }
}
