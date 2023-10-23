using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
   
    public class Invoice : BaseEntity
    {

        public Invoice() {
            ServiceItems = new HashSet<ServiceItem>();
        }

        public string InvoiceCode { get; set; }
		public bool IsApproved { get; set; }
		public int OldElectricNumber { get; set; }
        public int OldWaterNumber { get; set; }
        public int NewElectricNumber { get; set; }
        public int NewWaterNumber { get; set; }
        public decimal ElectricityCosts { get; set; }
        public decimal WaterCosts { get; set; }
        public decimal TotalPrice { get; set; }

        public int ContractId { get; set; }
        public Contract Contract { get; set; }
        public ICollection<ServiceItem> ServiceItems { get; set; }

    }
}
