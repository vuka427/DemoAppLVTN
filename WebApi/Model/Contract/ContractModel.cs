


using Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Model.Contract
{
    public class ContractModel
    {
        public string A_Lessor { get; set; }
        public string A_DateOfBirth { get; set; }
        public string A_Cccd { get; set; }
        public string A_DateOfIssuance { get; set; }
        public string A_PlaceOfIssuance { get; set; }
        public string A_PermanentAddress { get; set; }
        public string A_Phone { get; set; }
        public string B_Lessee { get; set; }
        public string B_DateOfBirth { get; set; }
        public string B_Cccd { get; set; }
        public string B_DateOfIssuance { get; set; }
        public string B_PlaceOfIssuance { get; set; }
        public string B_PermanentAddress { get; set; }
        public string B_Phone { get; set; }
        public decimal RentalPrice { get; set; }
        public int DurationOfHouseLease { get; set; }
        public string CommencingOn { get; set; }
        public string EndingOn { get; set; }
        public string Status { get; set; }
        public int BranchId { get; set; }
        public int AreaId { get; set; }
        public int RoomId { get; set; }
        public decimal Deposit { get; set; }
    }
}
