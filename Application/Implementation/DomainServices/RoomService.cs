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
        private readonly IRoomRepository _roomRepository;
        private readonly IPostNewRepository _postNewRepository;
        private readonly IDeviceRepository _deviceRepository;
        private readonly IRoomIndexRepository _roomIndexRepository;
        private readonly IImageRoomRepository _imageRoomRepository;

        public RoomService(IUnitOfWork unitOfWork, IBranchRepository branchRepository, IAreaRepository areaRepository, ILandlordRepository landlordRepository, IRoomRepository roomRepository, IPostNewRepository postNewRepository, IDeviceRepository deviceRepository, IRoomIndexRepository roomIndexRepository, IImageRoomRepository imageRoomRepository)
        {
            _unitOfWork=unitOfWork;
            _branchRepository=branchRepository;
            _areaRepository=areaRepository;
            _landlordRepository=landlordRepository;
            _roomRepository=roomRepository;
            _postNewRepository=postNewRepository;
            _deviceRepository=deviceRepository;
            _roomIndexRepository=roomIndexRepository;
            _imageRoomRepository=imageRoomRepository;
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
             var deleteroom = _roomRepository.FindById(roomid,r=>r.PostNews,r=>r.Devices,r=>r.RoomIndexs,r=>r.ImageRooms);
            if (deleteroom == null) { return new AppResult { Success = false, Message="Không tìm phòng !" }; }
            var area = _areaRepository.FindById(deleteroom.AreaId);
            if (area == null) { return new AppResult { Success = false, Message="Không tìm thấy nhà trọ !" }; }
            var branh = _branchRepository.FindById(area.BranchId);
            if (branh == null || branh.LandlordId != landlordId) { return new AppResult { Success = false, Message="Không tìm thấy nhà trọ !" }; }
            try
            {
                _postNewRepository.RemoveMultiple(deleteroom.PostNews.ToList());
                _deviceRepository.RemoveMultiple(deleteroom.Devices.ToList());
                _roomIndexRepository.RemoveMultiple(deleteroom.RoomIndexs.ToList());
                _imageRoomRepository.RemoveMultiple(deleteroom.ImageRooms.ToList());

                _roomRepository.Remove(deleteroom);
            }
            catch
            {
                return new AppResult { Success = false, Message="lỗi không thể xóa phòng !" };
            }
           

            return new AppResult { Success = true, Message="Ok" };
        }

        public Room GetRoomById(int landlordId, int roomid)
        {
            var room = _roomRepository.FindById(roomid,r=>r.Devices,r=>r.ImageRooms);
            if (room == null) { return new Room(); }
            var area = _areaRepository.FindById(room.AreaId);
            if (area == null) { return new Room(); }
            var branh = _branchRepository.FindById(area.BranchId);
            if (branh == null || branh.LandlordId != landlordId) { return new Room(); }
           
            return room;
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public AppResult UpdateRoom(int landlordId, Room room)
        {
            var landlord = _landlordRepository.FindById(landlordId, l => l.User);
            if (landlord == null) { return new AppResult { Success = false, Message="Không tìm thấy người dùng !" }; }
            var editRoom = _roomRepository.FindById(room.Id, r => r.Devices);
            if (editRoom == null) { return new AppResult { Success = false, Message="Không tìm phòng !" }; }


            editRoom.RoomNumber = room.RoomNumber;
            editRoom.IsMezzanine = room.IsMezzanine;
            editRoom.Price = room.Price;
            editRoom.MaxMember = room.MaxMember;
            editRoom.Acreage =  room.Acreage;
            editRoom.UpdatedBy = landlord.User.UserName??"";
            editRoom.UpdatedDate = DateTime.Now;

            try
            {
           
            var removeDevices = new List<Device>();

            foreach (var deviveItem in editRoom.Devices)
            {
                var item = room.Devices.Where(d=>d.Id == deviveItem.Id).FirstOrDefault();
                if (item != null)
                {
                    deviveItem.DeviceName = item.DeviceName;
                    deviveItem.Quantity = item.Quantity;
                    deviveItem.Description= item.Description;
                    deviveItem.UpdatedDate = DateTime.Now;

                }
                else
                {
                       
                        removeDevices.Add(deviveItem);
                }
                
            }
                foreach (var deviveItem in room.Devices)
                {
                    
                    if (deviveItem.Id == 0)
                    {
                        deviveItem.RoomId= room.Id;
                        deviveItem.CreatedDate = DateTime.Now;
                        deviveItem.CreatedBy=landlord.User.UserName??"";
                        deviveItem.UpdatedDate = DateTime.Now;
                        deviveItem.UpdatedBy=landlord.User.UserName??"";

                        _deviceRepository.Add(deviveItem);
                    }
                   

                }


                _deviceRepository.RemoveMultiple(removeDevices);
                _roomRepository.Update(editRoom);
                return new AppResult { Success = true, Message="ok" };
            }
            catch
            {
                return new AppResult { Success = false, Message="Không thêm được khu vực !" };
            }

 
        }

        public AppResult UploadRoomImage(int landlordId, int roomid, string fileName)
        {
            throw new NotImplementedException();
        }

        public AppResult UploadRoomImages(int landlordId, int roomid, string[] fileNames)
        {
            var room = _roomRepository.FindById(roomid);
            if(room == null) { return new AppResult { Success = false, Message="Không tìm phòng !" }; }

            var area = _areaRepository.FindById(room.AreaId);
            if (area == null) { return new AppResult { Success = false, Message="Không tìm thấy khu vực !" }; }
            var branh = _branchRepository.FindById(area.BranchId);
            if (branh == null || branh.LandlordId != landlordId) { return new AppResult { Success = false, Message="Không tìm thấy nhà trọ !" }; }

           
            foreach(var filename in fileNames)
            {
                _imageRoomRepository.Add(new ImageRoom
                    {
                     RoomId = room.Id,
                     Name = filename,
                     Url = filename,
                     CreatedBy = room.CreatedBy,
                     CreatedDate = DateTime.Now,
                     UpdatedBy = room.UpdatedBy,
                     UpdatedDate= DateTime.Now
                    });
            }
            
            return new AppResult { Success = true, Message="Ok" };
        }
    }
}
