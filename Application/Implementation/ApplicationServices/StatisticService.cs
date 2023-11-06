using Application.DTOs.Statistic;
using Application.Interface.ApplicationServices;
using Domain.Entities;
using Domain.Enum;
using Domain.Interface;
using Domain.IRepositorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Implementation.ApplicationServices
{
    public class StatisticService : IStatisticService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBranchRepository _branchRepository;
        private readonly IAreaRepository _areaRepository;
        private readonly ILandlordRepository _landlordRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IContractRepository _contractRepository;
        private readonly IMemberRepository _memberRepository;

        public StatisticService(IUnitOfWork unitOfWork, IBranchRepository branchRepository, IAreaRepository areaRepository, ILandlordRepository landlordRepository, IServiceRepository serviceRepository, IRoomRepository roomRepository, IContractRepository contractRepository, IMemberRepository memberRepository)
        {
            _unitOfWork=unitOfWork;
            _branchRepository=branchRepository;
            _areaRepository=areaRepository;
            _landlordRepository=landlordRepository;
            _serviceRepository=serviceRepository;
            _roomRepository=roomRepository;
            _contractRepository=contractRepository;
            _memberRepository=memberRepository;
        }

        public GeneralStatisticDto GetGeneralStatistic(int landlordid)
        {
            var result = new GeneralStatisticDto();


            var banches = _branchRepository.FindAll(b => b.LandlordId == landlordid, b => b.Areas).ToList();

            result.TotalBranch = banches.Count;

            int totalRoom = 0;
            int rentalRoom = 0;
            

            foreach (var branch in banches)
            {
                foreach (var area in branch.Areas)
                {
                    var rooms = _roomRepository.FindAll(r => r.AreaId == area.Id);
                    totalRoom += rooms.Count();
                    rentalRoom += rooms.Where(r=>r.Status == RoomStatus.Inhabited).Count();
                }
            }

            result.TotalRoom = totalRoom;
            result.RentalRoom = new int[] {rentalRoom ,totalRoom - rentalRoom};
           

            var contracts = _contractRepository.FindAll(c => c.LandlordId == landlordid && c.Status == ContractStatus.Active , c=>c.Invoices, c=>c.Members);
            result.TotalCustomer = contracts.SelectMany(c => c.Members).Where(m => m.IsActive == true).Count();
            result.Permanent = contracts.SelectMany(c => c.Members).Where(m => m.IsActive == true && ( m.IsPermanent == false || ( m.IsPermanent == true && m.PermanentDate < DateTime.Now ) )).Count();



            return result;
        }
    }
}
