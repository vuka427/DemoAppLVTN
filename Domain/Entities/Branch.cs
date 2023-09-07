using Domain.Common;
using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Branch : BaseEntity
    {
        public string BranchName { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public decimal ElectricityCosts { get; set; }
        public decimal WaterCosts { get; set; }
        public decimal InternetCosts { get; set; }
        public decimal GarbageColletionFee { get; set; }
        public string InternalRegulation { get; set; }
        public HouseType HouseType { get; set; }

    }
}
