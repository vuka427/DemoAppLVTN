using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Interface.ApplicationServices
{
    public interface IFeedbackService
    {
        IQueryable<Message> GetAllMessage(int landlordid);
    }
}
