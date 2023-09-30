﻿using Application.Interface.IDomainServices;
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
        private readonly IServiceRepository _serviceRepository;
        private readonly IRoomRepository _roomRepository;

        public BranchService(IUnitOfWork unitOfWork, IBranchRepository branchRepository, IAreaRepository areaRepository, ILandlordRepository landlordRepository, IServiceRepository serviceRepository, IRoomRepository roomRepository)
        {
            _unitOfWork=unitOfWork;
            _branchRepository=branchRepository;
            _areaRepository=areaRepository;
            _landlordRepository=landlordRepository;
            _serviceRepository=serviceRepository;
            _roomRepository=roomRepository;
        }

        public AppResult CreateArea(int landlordId, int branchId, Area area)
        {
            var branch = _branchRepository.FindAll(b => b.Id==branchId && b.LandlordId == landlordId).FirstOrDefault();

            if(branch ==null) return new AppResult { Success = false, Message="Không tìm thấy nhà trọ!" };
            var landlord = _landlordRepository.FindById(landlordId, l => l.User);
            if (landlord == null) { return new AppResult { Success = false, Message="Không tìm thấy người dùng !" }; }
            

            area.BranchId=branchId;
            area.CreatedBy=landlord.User.UserName??"";
            area.CreatedDate=DateTime.Now;
            area.UpdatedBy=landlord.User.UserName??"";
            area.UpdatedDate=DateTime.Now;
            

            try
            {
                _areaRepository.Add(area);
                return new AppResult { Success = true, Message="" };
            }
            catch
            {
                return new AppResult { Success = false, Message="Không thêm được khu vực!" };
            }


        }

        public AppResult CreateBranch(int landlordId, Branch branch)
        {
            var landlord = _landlordRepository.FindById(landlordId, l=>l.User );
            if(landlord == null) { return new AppResult { Success = false, Message="Không tìm thấy người dùng !"}; }
            branch.LandlordId = landlordId;
            branch.LandlordId = landlord.Id;
            branch.CreatedBy = landlord.User.UserName??"";
            branch.CreatedDate = DateTime.Now;
            branch.UpdatedBy = landlord.User.UserName??"";
            branch.UpdatedDate = DateTime.Now;

            foreach(var serviceItem in branch.Services)
            {
                serviceItem.CreatedBy = landlord.User.UserName??"";
                serviceItem.CreatedDate = DateTime.Now;
                serviceItem.UpdatedBy = landlord.User.UserName??"";
                serviceItem.UpdatedDate = DateTime.Now;
            }
            try
            {
                _branchRepository.Add(branch);
            }
            catch
            {
                return new AppResult { Success = false, Message="Không tìm thấy người dùng !"};
            }
            
            return new AppResult { Success = true, Message="oK"};
        }

        public AppResult DeleteBranch(int landlordId, int id)
        {
            
            var deletebranch = _branchRepository.FindAll(b => b.Id==id && b.LandlordId == landlordId, b=>b.Services).FirstOrDefault();
            if (deletebranch == null) { return new AppResult { Success = false, Message="Không tìm thấy nhà trọ !" }; }


            foreach (var serviceItem in deletebranch.Services)
            {
                _serviceRepository.Remove(serviceItem);
            }
            _branchRepository.Remove(deletebranch);

            return new AppResult { Success = false, Message="Không tìm thấy người dùng !" };

        }

        public Branch GetBranchById(int landlordId, int id)
        {
            return _branchRepository.FindAll(b=>b.LandlordId == landlordId && b.Id == id, b=>b.Areas ).FirstOrDefault();
        }

        public IQueryable<Branch> GetBranches(int landlordId)
        {
            var result  = _branchRepository.FindAll(b=>b.LandlordId == landlordId, b=>b.Areas);
            
            return result;
            
        }

        public IQueryable<Branch> GetBranchWithRoom(int landlordId)
        {
            var result = _branchRepository.FindAll(b => b.LandlordId == landlordId,b=>b.Areas);
            foreach (var branch in result)
            {
                foreach (var area in branch.Areas)
                {
                    area.Rooms = _roomRepository.FindAll(r=>r.AreaId==area.Id).ToList() ;
                }
            }

            return result;
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }
    }
}
