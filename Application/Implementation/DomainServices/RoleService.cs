using Application.Interface.IDomainServices;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Implementation.DomainServices
{
    public class RoleService : IRoleService
    {
        private RoleManager<AppRole> _roleManagement;

        public RoleService(RoleManager<AppRole> roleManagement)
        {
            _roleManagement = roleManagement;
        }

        public async Task<IdentityResult> AddRoleAsync(string name, string description)
        {
        
           var result = await  _roleManagement.CreateAsync(new AppRole() { Name = name, Description = description});

           return result;
        }
    }
}
