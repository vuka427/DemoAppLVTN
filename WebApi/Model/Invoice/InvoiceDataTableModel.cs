namespace WebApi.Model.Invoice
{
	public class InvoiceDataTableModel
	{
		
		public int Id { get; set; }
		public int Index { get; set; }
		public string Lessee { get; set; }
		public string RoomNumber { get; set; }
		public string BranchName { get; set; }
		public string InvoiceCode { get; set; }
		public bool IsApproved { get; set; }
		public decimal TotalPrice { get; set; }
		public int ContractId { get; set; }
		public string Year { get; set; }
		public string Month { get; set; }
	}
}
