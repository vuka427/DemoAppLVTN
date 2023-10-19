﻿using Domain.Common;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Interface.ApplicationServices
{
    public interface IContractService
    {
        AppResult CreateContract(int landlordId, Contract contract);
        IQueryable<Contract> GetContract(int landlordId);
		Contract GetContractById(int landlordId, int contractId);
		void SaveChanges();
        void Dispose();
    }
}
