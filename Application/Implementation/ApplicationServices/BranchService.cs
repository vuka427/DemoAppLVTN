using Application.Interface.ApplicationServices;
using Domain.Common;
using Domain.Entities;
using Domain.Enum;
using Domain.Interface;
using Domain.IRepositorys;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Implementation.ApplicationServices
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
            _unitOfWork = unitOfWork;
            _branchRepository = branchRepository;
            _areaRepository = areaRepository;
            _landlordRepository = landlordRepository;
            _serviceRepository = serviceRepository;
            _roomRepository = roomRepository;
        }

        public AppResult CreateArea(int landlordId, int branchId, Area area)
        {
            var branch = _branchRepository.FindAll(b => b.Id == branchId && b.LandlordId == landlordId).FirstOrDefault();

            if (branch == null) return new AppResult { Success = false, Message = "Không tìm thấy nhà trọ!" };
            var landlord = _landlordRepository.FindById(landlordId, l => l.User);
            if (landlord == null) { return new AppResult { Success = false, Message = "Không tìm thấy người dùng !" }; }


            area.BranchId = branchId;
            area.CreatedBy = landlord.User.UserName ?? "";
            area.CreatedDate = DateTime.Now;
            area.UpdatedBy = landlord.User.UserName ?? "";
            area.UpdatedDate = DateTime.Now;



            try
            {
                _areaRepository.Add(area);
                return new AppResult { Success = true, Message = "" };
            }
            catch
            {
                return new AppResult { Success = false, Message = "Không thêm được khu vực!" };
            }


        }

        public AppResult CreateBranch(int landlordId, Branch branch)
        {
            var landlord = _landlordRepository.FindById(landlordId, l => l.User);
            if (landlord == null) { return new AppResult { Success = false, Message = "Không tìm thấy người dùng !" }; }
            branch.LandlordId = landlordId;
            branch.LandlordId = landlord.Id;
            branch.CreatedBy = landlord.User.UserName ?? "";
            branch.CreatedDate = DateTime.Now;
            branch.UpdatedBy = landlord.User.UserName ?? "";
            branch.UpdatedDate = DateTime.Now;

            foreach (var serviceItem in branch.Services)
            {
                serviceItem.CreatedBy = landlord.User.UserName ?? "";
                serviceItem.CreatedDate = DateTime.Now;
                serviceItem.UpdatedBy = landlord.User.UserName ?? "";
                serviceItem.UpdatedDate = DateTime.Now;
            }
            try
            {
                _branchRepository.Add(branch);
            }
            catch
            {
                return new AppResult { Success = false, Message = "Không tìm thấy người dùng !" };
            }

            return new AppResult { Success = true, Message = "oK" };
        }

        public AppResult DeleteArea(int landlordId, int id)
        {
            var deleteArea = _areaRepository.FindById(id);
            if (deleteArea == null) { return new AppResult { Success = false, Message = "Không tìm thấy khu vực !" }; }

            var branch = _branchRepository.FindById(deleteArea.BranchId);
            if (branch == null || branch.LandlordId != landlordId) { return new AppResult { Success = false, Message = "Không tìm thấy nhà trọ !" }; }

            _areaRepository.Remove(deleteArea);

            return new AppResult { Success = true, Message = "ok" };
        }



        public AppResult DeleteBranch(int landlordId, int id)
        {

            var deletebranch = _branchRepository.FindAll(b => b.Id == id && b.LandlordId == landlordId, b => b.Services).FirstOrDefault();
            if (deletebranch == null) { return new AppResult { Success = false, Message = "Không tìm thấy nhà trọ !" }; }

            if (deletebranch.Services != null && deletebranch.Services.Count > 0)
            {
                _serviceRepository.RemoveMultiple(deletebranch.Services.ToList());
            }
            _branchRepository.Remove(deletebranch);

            return new AppResult { Success = true, Message = "Ok" };

        }

        public Area GetAreaById(int landlordId, int id)
        {
            var area = _areaRepository.FindById(id, a => a.Rooms);
            if (area == null) { return new Area(); }
            var branh = _branchRepository.FindById(area.BranchId);
            if (branh == null || branh.LandlordId != landlordId) { return new Area(); }

            return area;
        }

        public Branch GetBranchById(int landlordId, int id)
        {
            var branch = _branchRepository.FindAll(b => b.LandlordId == landlordId && b.Id == id, b => b.Areas).FirstOrDefault();

            foreach (var area in branch.Areas)
            {
                area.Rooms = _roomRepository.FindAll(r => r.AreaId == area.Id).ToList();
            }

            return branch ?? new Branch();
        }

		public Branch GetBranchByRoomId(int landlordId, int roomid)
		{
            var room = _roomRepository.FindById(roomid, r=>r.Area);
            if (room == null) { return null; }
            var branch = _branchRepository.FindById(room.Area.BranchId, b => b.Services);
            if (branch != null && branch.LandlordId == landlordId) {  return branch;}
            return null;
           
		}

		public IQueryable<Branch> GetBranches(int landlordId)
        {
            var result = _branchRepository.FindAll(b => b.LandlordId == landlordId, b => b.Areas);

            return result;

        }

        public ICollection<Branch> GetBranchWithRoom(int landlordId, string roomStatus)
        {
            var status = (roomStatus=="empty")? RoomStatus.Empty: ((roomStatus=="inhabited" )? RoomStatus.Inhabited : RoomStatus.Repair);

            var result = _branchRepository.FindAll(b => b.LandlordId == landlordId, b => b.Areas).ToList();

            if(roomStatus == "none")
            {
                foreach (var branch in result)
                {
                    foreach (var area in branch.Areas)
                    {
                        area.Rooms = _roomRepository.FindAll(r => r.AreaId == area.Id , r=>r.Contracts).ToList();
                    }
                }
            }
            else
            {
				foreach (var branch in result)
				{
					foreach (var area in branch.Areas)
					{
						area.Rooms = _roomRepository.FindAll(r => r.AreaId == area.Id && r.Status==status, r => r.Contracts).ToList();
					}
				}
			}
            

            return result;
        }

        public ICollection<Branch> GetBranchWithRoomIndex(int landlordId)
        {
            var result = _branchRepository.FindAll(b => b.LandlordId == landlordId, b => b.Areas).ToList();
            foreach (var branch in result)
            {
                foreach (var area in branch.Areas)
                {
                    area.Rooms = _roomRepository.FindAll(r => r.AreaId == area.Id, r => r.RoomIndexs, r => r.Contracts).ToList();
                }
            }

            return result;
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public AppResult UpdateArea(int landlordId, int branchId, Area area)
        {
            var branch = _branchRepository.FindAll(b => b.Id == branchId && b.LandlordId == landlordId, b => b.Areas).FirstOrDefault();

            if (branch == null) return new AppResult { Success = false, Message = "Không tìm thấy nhà trọ!" };
            var landlord = _landlordRepository.FindById(landlordId, l => l.User);
            if (landlord == null) { return new AppResult { Success = false, Message = "Không tìm thấy người dùng !" }; }
            var updateArea = branch.Areas.Where(a => a.Id == area.Id).FirstOrDefault();
            if (updateArea == null) { return new AppResult { Success = false, Message = "Không tìm thấy dãy tầng !" }; }

            updateArea.AreaName = area.AreaName;
            updateArea.Description = area.Description;
            updateArea.UpdatedBy = landlord.User.UserName ?? "";
            updateArea.UpdatedDate = DateTime.Now;


            try
            {
                _areaRepository.Update(updateArea);
                return new AppResult { Success = true, Message = "" };
            }
            catch
            {
                return new AppResult { Success = false, Message = "Không thêm được khu vực!" };
            }
        }



    }
}
