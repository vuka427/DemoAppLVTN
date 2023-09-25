using Domain.Common;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Interface.IDomainServices
{
    public interface IBranchService
    {
        AppResult CreateBranch(int landlordId, Branch branch);
        IQueryable<Branch> GetBranches(int landlordId);
        Branch GetBranchById(int landlordId, int id);

        AppResult DeleteBranch(int landlordId, int id);

        AppResult CreateArea(int branchId, Area area);
        void SaveChanges();
    }
}
