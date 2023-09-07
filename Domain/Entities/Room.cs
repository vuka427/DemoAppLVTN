using Domain.Common;
using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Room : BaseEntity
    {
        public int RoomNumber { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public float Length { get; set; }
        public bool IsMezzanine { get; set; }
        public decimal Price { get; set; }
        public RoomStatus Status { get; set; }
        public int MaxMember { get; set; }


    }
}
