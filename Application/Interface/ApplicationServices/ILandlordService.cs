using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interface.ApplicationServices
{
    public interface ILandlordService
    {
        void CreateNewLandlord(Landlord landlord);
        Landlord GetLandlordByUserId(string userid);
        Landlord GetLandlordById(int landlordid);
        void UpdateUser(Landlord landlord);
        void SaveChanges();

    }
}
