using Application.Interface.IDomainServices;
using Domain.Common;
using Domain.Entities;
using Domain.Interface;
using Domain.IRepositorys;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Implementation.DomainServices
{
    public class BranchService : IBranchService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBranchRepository _branchRepository;
        private readonly IAreaRepository _areaRepository;
        private readonly ILandlordRepository _landlordRepository;

        public AppResult CreateArea(int branchId, Area area)
        {
            throw new NotImplementedException();
        }

        public AppResult CreateBranch(int landlordId, Branch branch)
        {
            var landlord = _landlordRepository.FindById(landlordId);
            if(landlord == null) { return new AppResult { Success = false, Message="Không tìm thấy người dùng !"}; }
            branch.LandlordId = landlordId;
            try
            {
                _branchRepository.Add(branch);
                _unitOfWork.Commit();
            }
            catch
            {
                return new AppResult { Success = false, Message="Không tìm thấy người dùng !"};
            }
            
            return new AppResult { Success = true, Message="oK"};
        }

        public AppResult DeleteBranch(int landlordId, int id)
        {
            throw new NotImplementedException();
        }

        public Branch GetBranchById(int landlordId, int id)
        {
            throw new NotImplementedException();
        }

        public ICollection<Branch> GetBranches(int landlordId)
        {
            var landlord = _landlordRepository.FindById(landlordId);
            if (landlord == null) { return new List<Branch>(); }
            return  _branchRepository.FindAll(b=>b.LandlordId == landlordId).ToList();
            
        }
    }
}
