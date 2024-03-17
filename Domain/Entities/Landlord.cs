using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Landlord : BaseEntity
    {
        public Landlord() {
            EmailSends = new HashSet<EmailSend>();
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

        public ICollection<EmailSend> EmailSends { get; set; }
        public ICollection<Message> Messages { get; set; }
        public ICollection<Branch> Branchs { get; set;}
        public ICollection<Contract> Contracts { get; set; }
    }
}
