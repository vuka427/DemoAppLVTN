using Application.Interface.ApplicationServices;
using Domain.Entities;
using Domain.Interface;
using Domain.IRepositorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Implementation.ApplicationServices
{
    public class LandlordService : ILandlordService
    {
        private readonly ILandlordRepository _landlordRepository;
        private readonly IUnitOfWork _unitOfWork;

        public LandlordService(ILandlordRepository landlordRepository, IUnitOfWork unitOfWork)
        {
            _landlordRepository = landlordRepository;
            _unitOfWork = unitOfWork;
        }

        public void CreateNewLandlord(Landlord landlord)
        {
            _landlordRepository.Add(landlord);
        }

        public Landlord GetLandlordById(int landlordid)
        {
            return _landlordRepository.FindById(landlordid);
        }

        public Landlord GetLandlordByUserId(string userid)
        {
            return _landlordRepository.FindAll(l => l.UserId == userid).FirstOrDefault();
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public void UpdateUser(Landlord landlord)
        {
            _landlordRepository.Update(landlord);
        }
    }
}
