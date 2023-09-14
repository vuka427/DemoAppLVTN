using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interface.IDomainServices
{
    public interface ILandlordService
    {
        void CreateNewLandlord(Landlord landlord);
        Landlord GetLandlordByUserId(string userid);
        Landlord GetLandlordById(int landlordid);

        void SaveChanges();

    }
}
