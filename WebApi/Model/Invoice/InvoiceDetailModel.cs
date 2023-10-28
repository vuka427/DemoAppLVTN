namespace WebApi.Model.Invoice
{
	public class InvoiceDetailModel
	{
		public int Id { get; set; }
		public string InvoiceCode { get; set; }
		public string Lessee { get; set; }
		public string RoomNumber { get; set; }
		public string BranchName { get; set; }
		public bool IsApproved { get; set; }
		public int OldElectricNumber { get; set; }
		public int OldWaterNumber { get; set; }
		public int NewElectricNumber { get; set; }
		public int NewWaterNumber { get; set; }
		public decimal ElectricityCosts { get; set; }
		public decimal WaterCosts { get; set; }
		public decimal TotalPrice { get; set; }
		public int ContractId { get; set; }

		public ICollection<ServiceItemModel> ServiceItems { get; set; }


	}
}
