


using Domain.Entities;
using Domain.Enum;
using System.ComponentModel.DataAnnotations;
using WebApi.Model.Invoice;
using WebApi.Model.MemberModel;

namespace WebApi.Model.Contract
{
    public class ContractForRoomDetailModel
	{
	

		public int Index { get; set; }
        public string Id { get; set; }
        public string B_Lessee { get; set; }
        public string B_DateOfBirth { get; set; }
        public string B_Cccd { get; set; }
        public string B_DateOfIssuance { get; set; }
        public string B_PlaceOfIssuance { get; set; }
        public string B_PermanentAddress { get; set; }
        public string B_Phone { get; set; }
		public string B_Job { get; set; }
		public bool B_Gender { get; set; }
		public decimal RentalPrice { get; set; }
		public decimal ElectricityCosts { get; set; }
		public decimal WaterCosts { get; set; }
		public int DurationOfHouseLease { get; set; }
        public string CommencingOn { get; set; }
        public string EndingOn { get; set; }
        public string Status { get; set; }
        public int RoomNumber { get; set; }
        public string BranchName { get; set; }
        public string BranchAddress { get; set; }
        public string HouseType { get; set; }
        public string AreaName { get; set; }
        public float Acreage { get; set; }
        public bool IsMezzanine { get; set; }
        public string TermsOfContract { get; set; }
        public int BranchId { get; set; }
        public int AreaId { get; set; }
        public int RoomId { get; set; }
        public decimal Deposit { get; set; }

		public int? TenantId { get; set; }

		//public Tenant? Tenant { get; set; }

		public ICollection<InvoiceModel> Invoices { get; set; }
		public ICollection<MemberForDataTableModel> Members { get; set; }


	}
}
