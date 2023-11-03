using Application.DTOs.Contract;
using Application.Interface.ApplicationServices;
using Domain.Common;
using Domain.Entities;
using Domain.Enum;
using Domain.Interface;
using Domain.IRepositorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Application.Implementation.ApplicationServices
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
        private readonly IMemberRepository _memberRepository;

        public ContractService(IUnitOfWork unitOfWork, IBranchRepository branchRepository, IAreaRepository areaRepository, ILandlordRepository landlordRepository, IServiceRepository serviceRepository, IRoomRepository roomRepository, IContractRepository contractRepository, IMemberRepository memberRepository)
        {
            _unitOfWork=unitOfWork;
            _branchRepository=branchRepository;
            _areaRepository=areaRepository;
            _landlordRepository=landlordRepository;
            _serviceRepository=serviceRepository;
            _roomRepository=roomRepository;
            _contractRepository=contractRepository;
            _memberRepository=memberRepository;
        }

        public AppResult CreateContract(int landlordId, Contract contract)
        {

            if (contract.RoomId != null)
            {
                var room = _roomRepository.FindById(contract.RoomId.Value, r => r.Contracts);
                if (room == null) { return new AppResult { Success = false, Message = "Không tìm thấy phòng!" }; }
                var area = _areaRepository.FindById(room.AreaId);
                if (area == null) { return new AppResult { Success = false, Message = "Không tìm thấy khu vực!" }; }
                var branch = _branchRepository.FindById(area.BranchId);
                if (branch == null) { return new AppResult { Success = false, Message = "Không tìm thấy nhà trọ!" }; }
                var landlord = _landlordRepository.FindById(landlordId, l => l.User);

                if (landlord == null) { return new AppResult { Success = false, Message = "Không tìm thấy chủ trọ!" }; }

                room.Status = RoomStatus.Inhabited;

                contract.ContractCode = Guid.NewGuid().ToString();
                contract.LandlordId = landlordId;
                contract.RoomId = room.Id;
                contract.BranchName = branch.BranchName;
                contract.BranchId = branch.Id;
                contract.HouseType = branch.HouseType;
                contract.AreaName = area.AreaName;
                contract.RoomNumber = room.RoomNumber;
                contract.IsMezzanine = room.IsMezzanine;
                contract.Acreage = room.Acreage;
                contract.Status = ContractStatus.Active;
                contract.CreatedDate = DateTime.Now;
                contract.CreatedBy = landlord.User.UserName ?? "";
                contract.UpdatedDate = DateTime.Now;
                contract.UpdatedBy = landlord.User.UserName ?? "";

                var member = new Member()
                {
                    FullName  = contract.B_Lessee,
                    Gender = contract.B_Gender,
                    DateOfBirth = contract.B_DateOfBirth,
                    Cccd = contract.B_Cccd,
                    DateOfIssuance = contract.B_DateOfIssuance,
                    PlaceOfIssuance = contract.B_PlaceOfIssuance,
                    IsRepresent = true,
                    PermanentAddress = contract.B_PermanentAddress,
                    Job = contract.B_Job,
                    Phone = contract.B_Phone,
                    IsActive = true,
                    CommencingOn = contract.CommencingOn,
                    EndingOn = contract.EndingOn,

                    CreatedBy = contract.CreatedBy,
                    CreatedDate = contract.CreatedDate,
                    UpdatedBy = contract.UpdatedBy,
                    UpdatedDate = contract.UpdatedDate,
                };

                contract.Members.Add(member);

                try
                {
                    foreach (var oldContractItem in room.Contracts)
                    {
                        if (oldContractItem != null && oldContractItem.Status == ContractStatus.Active)
                        {
                            oldContractItem.Status = ContractStatus.Expirat;
                            oldContractItem.UpdatedBy = landlord.User.UserName ?? "";
                            oldContractItem.UpdatedDate = DateTime.Now;
                            _contractRepository.Update(oldContractItem);
                        }
                    }

                    _roomRepository.Update(room);
                    _contractRepository.Add(contract);
                    
                    return new AppResult { Success = true, Message = "ok" };
                }
                catch
                {
                    return new AppResult { Success = false, Message = "Lỗi không thêm đc hợp đồng!" };
                }

            }

            return new AppResult { Success = false, Message = "Lỗi không thêm đc hợp đồng!" };

        }



        public IQueryable<Contract> GetContract(int landlordId)
        {
            return _contractRepository.FindAll(c => c.LandlordId == landlordId);
        }

		public Contract GetContractById(int landlordId, int contractId)
		{

            var contract =  _contractRepository.FindById(contractId);
            if (contract != null && contract.LandlordId == landlordId)
            {
                return contract;
            }

            return null;
		}

		public void SaveChanges()
        {
            _unitOfWork.Commit();
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_unitOfWork != null) _unitOfWork.Dispose();

            }
        }

		public Contract GetContractByRoomId(int landlordId, int RoomId)
		{
			var room = _roomRepository.FindById(RoomId, r => r.Contracts);
			if (room == null) { return null; }
			var contract = room.Contracts.FirstOrDefault(r => r.Status == ContractStatus.Active && r.LandlordId == landlordId);
			if (contract == null) { return null; }
            return contract;

		}

		public bool ContractToEnd(int landlordId, int contractId)
		{
			var contract = _contractRepository.FindById(contractId,c=>c.Room);
			if (contract != null && contract.LandlordId == landlordId)
			{
                contract.Status = ContractStatus.Expirat;
                if (contract.RoomId!=null)
                {  
                    var room = _roomRepository.FindById(contract.RoomId.Value);
                    if (room != null) { 
                        room.Status = RoomStatus.Empty;
                        _roomRepository.Update(room);
                    }
                }
              
                _contractRepository.Update(contract);
                return true;
			}

            return false;
			
		}

        public ICollection<Member> GetMemberOfDataTable(int landlordId, string status, int branchid)
        {
            bool isActive = false;
            isActive = (status =="deactive") ? false : true;

            var contract = _contractRepository.FindAll(c => c.LandlordId==landlordId, c => c.Members).ToList();

            if (contract == null)
            {
                return new List<Member>();
            }

            IEnumerable<Member> allIMember = new List<Member>();
            IEnumerable<Member> Member = new List<Member>(); ;

            if (branchid==0) //branch
            {
                allIMember = contract.SelectMany(c => c.Members);
            }
            else
            {
                allIMember = contract.Where(c => c.BranchId==branchid).SelectMany(c => c.Members);
            }

            if (status != "none" ) //status
            {
                Member = allIMember.Where(i => i.IsActive == isActive);
            }
            else
            {
                Member = allIMember; //all
            }

            foreach (var item in Member)
            {
                item.Contract = contract.FirstOrDefault(c => c.Id==item.ContractId);
            }

            return Member.OrderByDescending(i => i.CreatedDate).ToList();

            
        }

        public AppResult CreateMember(int landlordId, int RoomId, Member member)
        {

            var room = _roomRepository.FindById(RoomId, r => r.Contracts);
            if (room == null) { return new AppResult { Success = false, Message = "Lỗi không tìm thấy phòng !" }; }
            var contract = room.Contracts.FirstOrDefault(r => r.Status == ContractStatus.Active && r.LandlordId == landlordId);
            if (contract == null) { return new AppResult { Success = false, Message = "Lỗi không tìm thấy hợp đồng !" }; }

            var currentMember = _memberRepository.FindAll(m => m.ContractId==contract.Id && m.IsActive == true);

            if (currentMember == null) { { return new AppResult { Success = false, Message = "Lỗi không tìm thấy hợp đồng !" }; } }

            if(currentMember.Count() >= room.MaxMember) { return new AppResult { Success = false, Message = "Phòng đã đủ số thành viên !" }; }

            member.IsActive = true;
            member.ContractId = contract.Id;
            member.IsRepresent =false;
            member.CreatedDate = DateTime.Now;
            member.CreatedBy =contract.CreatedBy;
            member.UpdatedDate = DateTime.Now;
            member.UpdatedBy=contract.UpdatedBy;

            _memberRepository.Add(member);


            return new AppResult { Success = true, Message = "Ok!" };
        }
    }
}
