﻿using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Service : BaseEntity
    {
        public string ServiceName { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }

        public int BranchId { get; set; }
        public Branch Branch { get; set; }
    }
}
