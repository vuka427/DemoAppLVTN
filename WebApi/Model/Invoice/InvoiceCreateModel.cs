﻿namespace WebApi.Model.Invoice
{
	public class InvoiceCreateModel
	{


		public int Id { get; set; }
        public int OldElectricNumber { get; set; }
        public int OldWaterNumber { get; set; }
        public int NewElectricNumber { get; set; }
		public int NewWaterNumber { get; set; }

		public int ContractId { get; set; }
		public int RoomId { get; set; }

		public DateTime LeaveDay { get; set; }
		public ICollection<ServiceItemModel>? Services { get; set; } 
	}
}
