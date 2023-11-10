using Domain.Common;
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
        IQueryable<Contract> GetContractForTenant(int tenantId);

        Contract GetContractById(int landlordId, int contractId);
        Contract GetContractByTenantId(int tenantId, int contractId);
        bool ContractToEnd(int landlordId, int contractId);
		Contract GetContractByRoomId(int landlordId, int RoomId);
        ICollection<Member> GetMemberOfDataTable(int landlordId, string status, int branchid);
        AppResult CreateMember(int landlordId, int RoomId, Member member);
        AppResult MemberLeave(int landlordId, int memberId, bool status);
        AppResult DeleteMember(int landlordId, int memberId);
        AppResult UpdateMember(int landlordId, Member member);
        Member GetMemberById(int landlordId, int memberId);
        bool LinkToTenant(int landlordId, int contractId, int tenantId);
        void SaveChanges();
        void Dispose();
    }
}
