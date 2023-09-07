using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Device : BaseEntity
    {
        public string DeviceName { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }

    }
}
