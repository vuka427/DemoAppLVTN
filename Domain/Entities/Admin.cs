using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Admin : BaseEntity
    {
        public string FullName { get; set; }
        public string AvatarUrl { get; set; }

        public int UserId { get; set; }
        public AppUser User { get; set; }
    }
}
