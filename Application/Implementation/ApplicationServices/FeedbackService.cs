using Application.Interface.ApplicationServices;
using Domain.Common;
using Domain.Entities;
using Domain.Enum;
using Domain.Interface;
using Domain.IRepositorys;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace Application.Implementation.ApplicationServices
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMessageRepository _messageRepository;
        private readonly ITenantRepository _tenantRepository;
        private readonly IContractRepository _contractRepository;

        public FeedbackService(IUnitOfWork unitOfWork, IMessageRepository messageRepository, ITenantRepository tenantRepository, IContractRepository contractRepository)
        {
            _unitOfWork=unitOfWork;
            _messageRepository=messageRepository;
            _tenantRepository=tenantRepository;
            _contractRepository=contractRepository;
        }

        public AppResult CreateFeedback(int tenantid, int contractid, Message message)
        {

            var contract = _contractRepository.FindById(contractid);
            if (contract != null)
            {
                var tenant = _tenantRepository.FindById(tenantid);
                if (tenant!=null)
                {
                    message.CreatedBy ="";
                    message.CreatedDate = DateTime.Now;
                    message.UpdatedDate = DateTime.Now;
                    message.UpdatedBy="";
                    message.Status = Domain.Enum.MessageStatus.Received;
                    message.TenantId = tenantid;
                    message.LandlordId= contract.LandlordId;
                    message.ReceiverName = contract.B_Lessee;
                    message.RoomName = "P."+ contract.RoomNumber +  ((contract.HouseType==HouseType.Row) ? ", dãy " : ", tầng ") + contract.AreaName +", " +contract.BranchName;

                    _messageRepository.Add(message);
                    return new AppResult { Success = true, Message = "ok!" };

                }

                return new AppResult { Success = false, Message = "Lỗi không tìm thấy người dùng !" };

            }

            return new AppResult { Success = false, Message = "Lỗi không tìm thấy hợp đồng !" };
        }

        public IQueryable<Message> GetAllMessage(int landlordid)
        {
           
            return _messageRepository.FindAll(e => e.LandlordId == landlordid);
        }

        public IQueryable<Message> GetAllMessageForTenant(int tenantid)
        {
            return _messageRepository.FindAll(e => e.TenantId == tenantid);
        }

        public void SaveChanges()
        {
           _unitOfWork.Commit();
        }
    }
}
