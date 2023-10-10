


using Domain.Enum;

namespace WebApi.Model.Contract
{
    public class ContractCreateModel
    {
        
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
       
       
        public int BranchId { get; set; }
        public int AreaId { get; set; }
        public int RoomId { get; set; }
        public decimal Deposit { get; set; }
    }
}
