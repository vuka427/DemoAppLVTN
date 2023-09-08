﻿using Domain.Common;
using Domain.Enums;
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
        public EmailStatus Status { get; set; }
    }
}
