using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class EmailSend : BaseEntity
    {
        public string EmailSender { get; set; }
        public string EmailReceiver { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Status { get; set; }
    }
}
