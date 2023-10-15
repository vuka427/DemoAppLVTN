using Domain.Common;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Interface.ApplicationServices
{
    public interface IBranchService
    {
        AppResult CreateBranch(int landlordId, Branch branch);

        IQueryable<Branch> GetBranches(int landlordId);
        ICollection<Branch> GetBranchWithRoom(int landlordId);
        ICollection<Branch> GetBranchWithRoomIndex(int landlordId);
        Branch GetBranchById(int landlordId, int id);
        Area GetAreaById(int landlordId, int id);

        AppResult DeleteBranch(int landlordId, int id);

        AppResult CreateArea(int landlordId, int branchId, Area area);
        AppResult UpdateArea(int landlordId, int branchId, Area area);
        AppResult DeleteArea(int landlordId, int id);
        void SaveChanges();
    }
}
