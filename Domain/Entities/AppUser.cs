using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }

        public DateTime? BirthDay { set; get; }

        public string Avatar { get; set; }
    }
}
