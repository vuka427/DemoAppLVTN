using Domain.Entities;
using Microsoft.VisualBasic;

namespace WebApi.Model.Invoice
{
	public class InvoiceModel
	{
		public int Id { get; set; }
		public string InvoiceCode { get; set; }
		public bool IsApproved { get; set; }
		public int OldElectricNumber { get; set; }
		public int OldWaterNumber { get; set; }
		public int NewElectricNumber { get; set; }
		public int NewWaterNumber { get; set; }
		public decimal RentalPrice { get; set; }
		public decimal ElectricityCosts { get; set; }
		public decimal WaterCosts { get; set; }
		public decimal TotalPrice { get; set; }
		public int ContractId { get; set; }
		public string Year { get; set; }
		public string Month { get; set; }
		public ICollection<ServiceItemModel> ServiceItems { get; set; }
	}
}
