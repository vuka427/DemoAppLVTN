using Application.Interface;
using Application.Interface.ApplicationServices;
using Domain.Entities;
using Domain.Interface;
using Domain.IRepositorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Implementation.ApplicationServices
{
    public class TenantService : ITenantService
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TenantService(ITenantRepository tenantRepository, IUnitOfWork unitOfWork)
        {
            _tenantRepository = tenantRepository;
            _unitOfWork = unitOfWork;
        }

        public void CreateNewTenant(Tenant tenant)
        {
            _tenantRepository.Add(tenant);
        }

        public void DeleteTenant(int tenantid)
        {
            
            _tenantRepository.Remove(tenantid);
        }

        public Tenant GetTenantById(int tenantid)
        {
            return _tenantRepository.FindById(tenantid);
        }

        public Tenant GetTenantByUserId(string userid)
        {
            return _tenantRepository.FindAll(t => t.UserId == userid).FirstOrDefault();
        }

        public void UpdateUser(Tenant tenant)
        {
            _tenantRepository.Update(tenant);
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }
    }
}
