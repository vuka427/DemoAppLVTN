using Domain.Common;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interface.IDomainServices
{
    public interface IRoomService
    {
        AppResult CreateRoom(int landlordId,int branchId, int areaId, Room room );
        AppResult UpdateRoom(int branchId, Room room);
        AppResult DeleteRoom(int landlordId,int roomid);

        void SaveChanges();

    }
}
