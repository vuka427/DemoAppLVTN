using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Area : BaseEntity
    {
        public string AreaName { get; set; }
        public string Description { get; set; }

        public int BranchId { get; set; }
        public Branch Branch { get; set; }

        public ICollection<Room> Rooms { get; set; }

    }
}
