using Application.Interface.ApplicationServices;
using Domain.Entities;
using Domain.IRepositorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Implementation.ApplicationServices
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IMessageRepository _messageRepository;

        public FeedbackService(IMessageRepository messageRepository)
        {
            _messageRepository=messageRepository;
        }

        public IQueryable<Message> GetAllMessage(int landlordid)
        {
           
            return _messageRepository.FindAll(e => e.LandlordId == landlordid);
          
        }
    }
}
