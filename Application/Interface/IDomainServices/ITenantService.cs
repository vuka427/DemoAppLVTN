using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interface.IDomainServices
{
    public interface ITenantService
    {
        void CreateNewTenant(Tenant tenant);
        Tenant GetTenantByUserId(string userid);
        Tenant GetTenantById(int tenantid);

        void SaveChanges();
    }
}
