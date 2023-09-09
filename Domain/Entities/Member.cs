using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Member : BaseEntity
    {
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Cccd { get; set; }
        public DateTime DateOfIssuance { get; set; }
        public string PlaceOfIssuance { get; set; }
        public string PermanentAddress { get; set; }
        public string Phone { get; set; }

        public int ContractId { get; set; }
        public Contract Contract { get; set; }
    }
}
