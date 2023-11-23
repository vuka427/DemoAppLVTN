using Application.Interface;
using Application.Interface.ApplicationServices;
using Application.Utility;
using Domain.Common;
using Domain.Entities;
using Domain.Enum;
using Domain.Interface;
using Domain.IRepositorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Implementation.ApplicationServices
{
    public class EmailService : IEmailService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISendMailService _sendMailService;
        private  IEmailSendRepository _emailSendRepository;

        public EmailService(IUnitOfWork unitOfWork, ISendMailService sendMailService, IEmailSendRepository emailSendRepository)
        {
            _unitOfWork=unitOfWork;
            _sendMailService=sendMailService;
            _emailSendRepository=emailSendRepository;
        }

        public IQueryable<EmailSend> GetAllEmail(int landlordid, int branchid)
        {


            return _emailSendRepository.FindAll(e=>e.LandlordId == landlordid);
        }

     


        public async Task<AppResult> SendMailCreateInvoice(string email, string receiverName,Contract contract, Invoice invoice)
        {
            var htmlMessage = GenerateInvoiceToHtmlMessage.ConverterCreateInvoiceToHtml(contract, invoice);

            var result = await _sendMailService.SendEmailAsync(email,"Thông báo tiền trọ", htmlMessage);

            var mailSave = new EmailSend {
                                TenantId = contract.TenantId,
                                LandlordId = contract.LandlordId,
                                ReceiverName = contract.B_Lessee,
                                RoomName=  "P."+ contract.RoomNumber +  ((contract.HouseType==HouseType.Row) ? ", dãy " : ", tầng ") + contract.AreaName +", " +contract.BranchName,
                                EmailReceiver = email,
                                EmailSender = "",
                                Title ="Thông báo tiền trọ",
                                Content = htmlMessage,
                                CreatedBy = invoice.CreatedBy,
                                CreatedDate= DateTime.Now,
                                UpdatedBy = invoice.UpdatedBy,
                                UpdatedDate= DateTime.Now
                            };

            if (result.Success)
            {
                mailSave.Status = Domain.Enums.EmailStatus.Successed;
            }
            else
            {
                mailSave.Status = Domain.Enums.EmailStatus.Failed;
            }

            try
            {


                _emailSendRepository.AddEmailSend(mailSave);

               

            } catch (Exception ex)
            {
                return new AppResult { Success = true, Message="không gửi mail đc" };
            }
           
            
            return new AppResult { Success = true , Message="ok" };
        }

        public async Task<AppResult> SendMailPayInvoice(string email, string receiverName, Contract contract, Invoice invoice)
        {
            var htmlMessage = GenerateInvoiceToHtmlMessage.ConverterPayInvoiceToHtml(contract, invoice);

            var result = await _sendMailService.SendEmailAsync(email, "Xác nhận đã thanh toán tiền trọ", htmlMessage);

            var mailSave = new EmailSend
            {
                TenantId = contract.TenantId,
                LandlordId = contract.LandlordId,
                ReceiverName = contract.B_Lessee,
                RoomName=  "P."+ contract.RoomNumber +  ((contract.HouseType==HouseType.Row) ? ", dãy " : ", tầng ") + contract.AreaName +", " +contract.BranchName,
                EmailReceiver = email,
                EmailSender = "",
                Title ="Xác nhận đã thanh toán tiền trọ",
                Content = htmlMessage,
                CreatedBy = invoice.CreatedBy,
                CreatedDate= DateTime.Now,
                UpdatedBy = invoice.UpdatedBy,
                UpdatedDate= DateTime.Now
            };

            if (result.Success)
            {
                mailSave.Status = Domain.Enums.EmailStatus.Successed;
            }
            else
            {
                mailSave.Status = Domain.Enums.EmailStatus.Failed;
            }

            try
            {

                _emailSendRepository.AddEmailSend(mailSave);

            }
            catch (Exception ex)
            {
                return new AppResult { Success = true, Message="không gửi mail đc" };
            }

            return new AppResult { Success = true, Message="ok" };
        }
    }
}
