using Application.DTOs.Statistic;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interface.ApplicationServices
{
     public interface IStatisticService
    {
        GeneralStatisticDto GetGeneralStatistic(int landlordid);
        StatisticForBranchDto GetBranchStatistic(int landlordid,int year, int branchid);
        StatisticForBranchDto GetTenatStatistic(int tenantid, int year, int contractid);
    }
}
