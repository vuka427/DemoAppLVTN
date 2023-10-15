using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interface.ApplicationServices
{
    public interface ITenantService
    {
        void CreateNewTenant(Tenant tenant);
        void DeleteTenant(int tenantid);
        Tenant GetTenantByUserId(string userid);
        Tenant GetTenantById(int tenantid);

        void SaveChanges();
    }
}
