using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Invoice : BaseEntity

    {
        public string InvoiceCode { get; set; }
        public string OldElectricNumber { get; set; }
        public string OldWaterNumber { get; set; }
        public string NewElectricNumber { get; set; }
        public string NewWaterNumber { get; set; }
        public decimal ElectricityCosts { get; set; }
        public decimal WaterCosts { get; set; }
        public decimal InternetCosts { get; set; }
        public decimal GarbageColletionFee { get; set; }
        public decimal TotalPrice { get; set; }


    }
}
