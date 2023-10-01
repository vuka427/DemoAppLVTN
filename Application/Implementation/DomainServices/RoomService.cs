using Application.Interface.IDomainServices;
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
    public class RoomService : IRoomService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IBranchRepository _branchRepository;
        private readonly IAreaRepository _areaRepository;
        private readonly ILandlordRepository _landlordRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IRoomRepository _roomRepository;

        public RoomService(IUnitOfWork unitOfWork, IBranchRepository branchRepository, IAreaRepository areaRepository, ILandlordRepository landlordRepository, IServiceRepository serviceRepository, IRoomRepository roomRepository)
        {
            _unitOfWork=unitOfWork;
            _branchRepository=branchRepository;
            _areaRepository=areaRepository;
            _landlordRepository=landlordRepository;
            _serviceRepository=serviceRepository;
            _roomRepository=roomRepository;
        }

        public AppResult CreateRoom(int landlordId, int branchId, int areaId, Room room)
        {

            var landlord = _landlordRepository.FindById(landlordId, l=>l.User);
            if (landlord == null) { return new AppResult { Success = false, Message="Không tìm thấy người dùng !" }; }

            var branch = _branchRepository.FindAll(b => b.Id == branchId && b.LandlordId == landlordId, b => b.Areas).FirstOrDefault();
            if(branch == null || branch.Areas == null) { return new AppResult { Success = false, Message="Không tìm thấy nhà trọ !" }; }

            var area = branch.Areas.Where(b=>b.Id == areaId).FirstOrDefault();
            if (area == null) { return new AppResult { Success = false, Message="Không tìm thấy nhà trọ !" }; }
            room.AreaId = areaId;
            room.Status = Domain.Enum.RoomStatus.Empty;
            room.CreatedDate = DateTime.Now;
            room.CreatedBy =landlord.User.UserName??"";
            room.UpdatedBy=landlord.User.UserName??"";
            room.UpdatedDate = DateTime.Now;

            foreach (var deviveItem in room.Devices)
            {
                deviveItem.CreatedDate = DateTime.Now;
                deviveItem.CreatedBy=landlord.User.UserName??"";
                deviveItem.UpdatedDate = DateTime.Now;
                deviveItem.UpdatedBy=landlord.User.UserName??"";
            }


            try
            {
                
                _roomRepository.Add(room);
                return new AppResult { Success = true, Message="ok" };
            }
            catch
            {
                return new AppResult { Success = false, Message="Không thêm được khu vực !" };
            }

        }

        public AppResult DeleteRoom(int landlordId, int roomid)
        {
            throw new NotImplementedException();
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public AppResult UpdateRoom(int branchId, Room room)
        {
            throw new NotImplementedException();
        }
    }
}
