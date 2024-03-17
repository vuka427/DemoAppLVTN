using Domain.Common;
using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Domain.Entities
{
    public class Branch : BaseEntity
    { 
        public Branch() {
            Areas = new HashSet<Area>();
            Services = new HashSet<Service>();
        }
        public string BranchName { get; set; }
        public string Description { get; set; }
        public int Province { get; set; }
        public int District { get; set; }
        public int Wards { get; set; }
        public string Address { get; set; }
        public decimal ElectricityCosts { get; set; }
        public decimal WaterCosts { get; set; }
        public string InternalRegulation { get; set; }
        public HouseType HouseType { get; set; }

        public int LandlordId { get; set; }
        public Landlord Landlord { get; set; }

        public ICollection<Area> Areas { get; set; }
        public ICollection<Service> Services { get; set; }
    }
}
