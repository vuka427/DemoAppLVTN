using Domain.Common;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interface.ApplicationServices
{
    public interface IRoomService
    {
        AppResult CreateRoom(int landlordId, int branchId, int areaId, Room room);
        AppResult UpdateRoom(int landlordId, Room room);
        AppResult DeleteRoom(int landlordId, int roomid);
        Room GetRoomById(int landlordId, int roomid);
        Room GetRoomForDetailById(int landlordId, int roomid);
        Room GetRoomDetailForTenantById(int tenantId, int roomid);
        ICollection<Contract> GetRoomForTenant(int tenantId);
        ImageRoom UploadRoomImage(int landlordId, int roomid, string fileName);
        AppResult UploadRoomImages(int landlordId, int roomid, string[] fileNames);
        AppResult DeleteImageRoom(int landlordId, int imageId);
        void SaveChanges();

    }
}
