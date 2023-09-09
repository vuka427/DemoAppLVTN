using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class ImageRoom : BaseEntity
    {
        public string Name { get; set; }
        public string Url { get; set; }

        public int RoomId { get; set; }
        public Room Room { get; set; }
    }
}
