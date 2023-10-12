﻿using Application.Interface.IDomainServices;
using Domain.Common;
using Domain.Entities;
using Domain.Interface;
using Domain.IRepositorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Implementation.DomainServices
{
    public class ContractService : IContractService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBranchRepository _branchRepository;
        private readonly IAreaRepository _areaRepository;
        private readonly ILandlordRepository _landlordRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IContractRepository _contractRepository;

        public ContractService(IUnitOfWork unitOfWork, IBranchRepository branchRepository, IAreaRepository areaRepository, ILandlordRepository landlordRepository, IServiceRepository serviceRepository, IRoomRepository roomRepository, IContractRepository contractRepository)
        {
            _unitOfWork=unitOfWork;
            _branchRepository=branchRepository;
            _areaRepository=areaRepository;
            _landlordRepository=landlordRepository;
            _serviceRepository=serviceRepository;
            _roomRepository=roomRepository;
            _contractRepository=contractRepository;
        }

        public AppResult CreateContract(int landlordId, Contract contract)
        {


            if (contract.RoomId!=null)
            { 
                
                var room = _roomRepository.FindById(contract.RoomId.Value);
                if (room==null) { return new AppResult { Success = false, Message="Không tìm thấy phòng!" }; }
                var area = _areaRepository.FindById(room.AreaId);
                if (area==null) { return new AppResult { Success = false, Message="Không tìm thấy khu vực!" }; }
                var branch = _branchRepository.FindById(area.BranchId);
                if (branch==null) { return new AppResult { Success = false, Message="Không tìm thấy nhà trọ!" }; }
                var landlord = _landlordRepository.FindById(landlordId,l=>l.User);
                
                if (landlord==null) { return new AppResult { Success = false, Message="Không tìm thấy chủ trọ!" }; }

                contract.LandlordId = landlordId;
                contract.RoomId = room.Id;
                contract.BranchAddress = branch.Address;
                contract.BranchName = branch.BranchName;
                contract.HouseType = branch.HouseType;
                contract.AreaName = area.AreaName;
                contract.RoomNumber = room.RoomNumber;
                contract.IsMezzanine = room.IsMezzanine;
                contract.Acreage = room.Acreage;
                contract.Status = Domain.Enum.ContractStatus.Active;
                contract.CreatedDate = DateTime.Now;
                contract.CreatedBy = landlord.User.UserName??"";
                contract.UpdatedDate = DateTime.Now;
                contract.UpdatedBy = landlord.User.UserName??"";
            }
            try
            {

                _contractRepository.Add(contract);

                return new AppResult { Success = true, Message="ok" };
            }
            catch
            {
                return new AppResult { Success = false, Message="Lỗi không thêm đc hợp đồng!" };
            }




           
        }

        public IQueryable<Contract> GetContract(int landlordId)
        {
            throw new NotImplementedException();
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }
    }
}