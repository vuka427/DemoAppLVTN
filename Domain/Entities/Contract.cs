using Domain.Common;
using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;


namespace Domain.Entities
{
    
    public class Contract : BaseEntity
    {
        public Contract() {
            Invoices = new HashSet<Invoice>();
            Members = new HashSet<Member>();
        }
        public string ContractCode { get; set; }
        public string A_Lessor { get; set; }
        public DateTime A_DateOfBirth { get; set; }
        public string A_Cccd { get; set; }
        public DateTime A_DateOfIssuance { get; set; }
        public string A_PlaceOfIssuance { get; set; }
        public string A_PermanentAddress { get; set; }
        public string A_Phone { get; set; }

        public string B_Lessee { get; set; }
        public DateTime B_DateOfBirth { get; set; }
        public string B_Cccd { get; set; }
        public DateTime B_DateOfIssuance { get; set; }
        public string B_PlaceOfIssuance { get; set; }
        public string B_PermanentAddress { get; set; }
        public string B_Phone { get; set; }

        public decimal RentalPrice { get; set; }
        public int DurationOfHouseLease { get; set; }
        public DateTime CommencingOn { get; set; }
        public DateTime EndingOn { get; set; }
        public ContractStatus Status { get; set; }
        public int RoomNumber { get; set; }
        public string BranchName { get; set; }
        public string BranchAddress { get; set; }
        public HouseType HouseType { get; set; }
        public string AreaName { get; set; }
        public float Acreage { get; set; }
        public bool IsMezzanine { get; set; }
        public decimal Deposit { get; set; }

        public string TermsOfContract { get; set; }

        public int? RoomId { get; set; }
        public Room? Room { get; set; }

        public int LandlordId { get; set; }
        public Landlord Landlord { get; set; }

        public int? TenantId { get; set; }

        public Tenant? Tenant { get; set; }

        public ICollection<Invoice> Invoices { get; set; }
        public ICollection<Member> Members { get; set; }


    }
}
