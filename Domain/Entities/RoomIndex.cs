using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class RoomIndex : BaseEntity
    {
        public int ElectricNumber { get; set; }
        public int WaterNumber { get; set; }

        public int RoomId { get; set; }
        public Room Room { get; set; }
    }
}
