using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface.ApplicationServices
{
    public interface IRoleService
    {
        Task<IdentityResult> AddRoleAsync(string name, string description);
    }
}
