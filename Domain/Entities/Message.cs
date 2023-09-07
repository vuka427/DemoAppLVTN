using Domain.Common;
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
        public string MessageType { get; set; }
    }
}
