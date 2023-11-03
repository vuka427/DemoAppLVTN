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
		Contract GetContractById(int landlordId, int contractId);
		bool ContractToEnd(int landlordId, int contractId);
		Contract GetContractByRoomId(int landlordId, int RoomId);
        ICollection<Member> GetMemberOfDataTable(int landlordId, string status, int branchid);
        AppResult CreateMember(int landlordId, int RoomId, Member member);
        void SaveChanges();
        void Dispose();
    }
}
