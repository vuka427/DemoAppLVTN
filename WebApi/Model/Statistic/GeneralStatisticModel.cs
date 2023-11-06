namespace WebApi.Model.Statistic
{
    public class GeneralStatisticModel
    {
        public int TotalBranch { get; set; }
        public int TotalRoom { get; set; }
        public int TotalCustomer { get; set; }
        public int TotalFeedBack { get; set; }
        public int TotalDebt { get; set;}

        public int[] Order { get; set; }
        public int[] RentalRoom { get; set; }
        public int Permanent { get; set; }
    }
}
