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
        public MessageStatus  Status { get; set; }

        public string ReceiverName { get; set; }
        public string RoomName { get; set; }

        public string BranchName { get; set; }
        public int TenantId { get; set; }
        public Tenant Tenant { get; set; }
        public int LandlordId { get; set; }
        public Landlord Landlord { get; set; }
    }
}
