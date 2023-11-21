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
        private readonly IInvoiceRepository _invoiceRepository;

        public StatisticService(IUnitOfWork unitOfWork, IBranchRepository branchRepository, IAreaRepository areaRepository, ILandlordRepository landlordRepository, IServiceRepository serviceRepository, IRoomRepository roomRepository, IContractRepository contractRepository, IMemberRepository memberRepository, IInvoiceRepository invoiceRepository)
        {
            _unitOfWork=unitOfWork;
            _branchRepository=branchRepository;
            _areaRepository=areaRepository;
            _landlordRepository=landlordRepository;
            _serviceRepository=serviceRepository;
            _roomRepository=roomRepository;
            _contractRepository=contractRepository;
            _memberRepository=memberRepository;
            _invoiceRepository=invoiceRepository;
        }

        public StatisticForBranchDto GetBranchStatistic(int landlordid, int year, int branchid)
        {
            var result = new StatisticForBranchDto();
            var currentYear = DateTime.Now.Year;
            if (year!=0)
            {
                currentYear = year;
            }

            int totalRoom = 0;
            int rentalRoom = 0;


            if (branchid==0) // lọc tất cả 
            {
                var banches = _branchRepository.FindAll(b => b.LandlordId == landlordid, b => b.Areas).ToList();

                foreach (var branch in banches)
                {
                    foreach (var area in branch.Areas)
                    {
                        var rooms = _roomRepository.FindAll(r => r.AreaId == area.Id);
                        totalRoom += rooms.Count();
                        rentalRoom += rooms.Where(r => r.Status == RoomStatus.Inhabited).Count();
                    }
                }

                result.RentalRoom = new int[] { rentalRoom, totalRoom - rentalRoom };

                var invoices = _invoiceRepository.FindAll(i => i.CreatedDate.Year == currentYear && i.IsApproved == true, i => i.Contract).Where(i => i.Contract.LandlordId == landlordid).ToList();

                for (int i =1 ;i<=12 ; i++ )
                {
                    decimal totalEarning = 0;
                    int totalElectric = 0;
                    int totalWanter = 0;

                    var invoiceInMonth = invoices.Where(iv => iv.CreatedDate.Month == i).ToList();
                   
                    foreach(var invoiceItem in invoiceInMonth)
                    {
                        totalEarning += invoiceItem.TotalPrice;
                        totalElectric += invoiceItem.NewElectricNumber - invoiceItem.OldElectricNumber;
                        totalWanter += invoiceItem.NewWaterNumber - invoiceItem.OldWaterNumber;

                    }

                    result.Earning[i-1] = totalEarning;
                    result.Electricity[i-1] = totalElectric;
                    result.Wanter[i-1] = totalWanter;

                }

                var members = _memberRepository.FindAll(i => i.CreatedDate.Year == currentYear, i => i.Contract).Where(i => i.Contract.LandlordId == landlordid).ToList();

                for (int i = 1; i<=12; i++)
                {
                    var memberInMonthCr = members.Where(iv => iv.CommencingOn.Month == i).ToList();
                    var memberInMonthUd = members.Where(iv => iv.UpdatedDate.Month == i).ToList();

                    int totalMemberIn = 0;
                    int totalMemberOut = 0;
                    

                    foreach (var memberItem in memberInMonthCr)
                    {
                        totalMemberIn++; 

                    }
                    foreach (var memberItem in memberInMonthUd)
                    {
                        if (memberItem.IsActive == false) { totalMemberOut++; }

                    }

                    result.MemberOut[i-1] = totalMemberOut;
                    result.MemberIn[i-1]  = totalMemberIn;

                }


            }
            else // lọc  theo nhà trọ
            {

                var banches = _branchRepository.FindAll(b => b.LandlordId == landlordid && b.Id == branchid, b => b.Areas).ToList();

                foreach (var branch in banches)
                {
                    foreach (var area in branch.Areas)
                    {
                        var rooms = _roomRepository.FindAll(r => r.AreaId == area.Id);
                        totalRoom += rooms.Count();
                        rentalRoom += rooms.Where(r => r.Status == RoomStatus.Inhabited).Count();
                    }
                }
                result.RentalRoom = new int[] { rentalRoom, totalRoom - rentalRoom };

                var invoices = _invoiceRepository.FindAll(i => i.CreatedDate.Year == currentYear && i.IsApproved == true, i => i.Contract).Where(i => i.Contract.LandlordId == landlordid && i.Contract.BranchId == branchid).ToList();

                for (int i = 1; i<=12; i++)
                {
                    decimal totalEarning = 0;
                    int totalElectric = 0;
                    int totalWanter = 0;

                    var invoiceInMonth = invoices.Where(iv => iv.CreatedDate.Month == i).ToList();

                    foreach (var invoiceItem in invoiceInMonth)
                    {
                        totalEarning += invoiceItem.TotalPrice;
                        totalElectric += invoiceItem.NewElectricNumber - invoiceItem.OldElectricNumber;
                        totalWanter += invoiceItem.NewWaterNumber - invoiceItem.OldWaterNumber;

                    }

                    result.Earning[i-1] = totalEarning;
                    result.Electricity[i-1] = totalElectric;
                    result.Wanter[i-1] = totalWanter;

                }

                var members = _memberRepository.FindAll(i => i.CreatedDate.Year == currentYear, i => i.Contract).Where(i => i.Contract.LandlordId == landlordid && i.Contract.BranchId == branchid).ToList();

                for (int i = 1; i<=12; i++)
                {

                    var memberInMonthCr = members.Where(iv => iv.CommencingOn.Month == i).ToList();
                    var memberInMonthUd = members.Where(iv => iv.UpdatedDate.Month == i).ToList();

                    int totalMemberIn = 0;
                    int totalMemberOut = 0;


                    foreach (var memberItem in memberInMonthCr)
                    {
                        totalMemberIn++;

                    }
                    foreach (var memberItem in memberInMonthUd)
                    {
                        if (memberItem.IsActive == false) { totalMemberOut++; }

                    }

                    result.MemberOut[i-1] = totalMemberOut;
                    result.MemberIn[i-1]  = totalMemberIn;

                }



            }




            return result;
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
