using Domain.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Domain.Entities
{
    public class Tenant : BaseEntity
    {
        public Tenant() { 
            EmailReceives = new HashSet<EmailSend>();
            Messages = new HashSet<Message>();
            Contracts = new HashSet<Contract>();
        }

        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Ccccd { get; set; }
        public string AvatarUrl { get; set; }

        public string UserId { get; set; }
        public AppUser User { get; set; }

        public ICollection<EmailSend> EmailReceives { get; set; }
        public ICollection<Message> Messages { get; set; }

        public ICollection<Contract> Contracts { get; set; }
    }
}
