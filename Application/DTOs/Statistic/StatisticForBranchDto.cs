using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Statistic
{
    public class StatisticForBranchDto
    {
        public StatisticForBranchDto()
        {
            RentalRoom = new int[2];
            Earning =new decimal[12];
            Electricity=new int[12];
            Wanter=new int[12];
            MemberIn=new int[12];
            MemberOut=new int[12];
            Rooms=new int[12];
        }

        public int[] RentalRoom { get; set; }  
        public decimal[] Earning { get; set; }
        public int[] Electricity { get; set; }
        public int[] Wanter { get; set; }
        public int[] MemberIn { get; set; }
        public int[] MemberOut { get; set; }
        public int[] Rooms { get; set; }

    }
}
