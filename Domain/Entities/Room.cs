using Domain.Common;
using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Room : BaseEntity
    {
        public Room() {
            Devices = new HashSet<Device>();
            RoomIndexs = new HashSet<RoomIndex>();
            ImageRooms = new HashSet<ImageRoom>();
            PostNews = new HashSet<PostNew>();
            Contracts = new HashSet<Contract>();
        }

        public int RoomNumber { get; set; }
        public float Acreage { get; set; }
       
        public bool IsMezzanine { get; set; }
        public decimal Price { get; set; }
        public RoomStatus Status { get; set; }
        public int MaxMember { get; set; }

        public int AreaId { get; set; }
        public Area Area { get; set; }

        public ICollection<Device> Devices { get; set; }
        public ICollection<RoomIndex> RoomIndexs { get; set; }
        public ICollection<ImageRoom> ImageRooms { get; set; }
        public ICollection<PostNew> PostNews { get; set; }
        public ICollection<Contract> Contracts { get; set; }




    }
}
