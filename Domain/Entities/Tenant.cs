using Domain.Common;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Domain.Entities
{
    public class Tenant : BaseEntity
    {
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Ccccd { get; set; }
        public string AvatarUrl { get; set; }

        public int UserId { get; set; }
        public AppUser User { get; set; }

    }
}
