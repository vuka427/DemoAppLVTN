using Domain.Common;
using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Message : BaseEntity
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Status { get; set; }
        public MessageType MessageType { get; set; }

        public int TenantId { get; set; }
        public Tenant Tenant { get; set; }
        public int LandlordId { get; set; }
        public Landlord Landlord { get; set; }
    }
}
