namespace WebApi.Model.Branch
{
    public class BranchCreateModel
    {
        public int Id { get; set; }
        public string BranchName { get; set; }
        public string Description { get; set; }
        public int Province { get; set; }
        public int District { get; set; }
        public int Wards { get; set; }
        public string Address { get; set; }
        public decimal ElectricityCosts { get; set; }
        public decimal WaterCosts { get; set; }
        public string InternalRegulation { get; set; }
        public ServiceModel[] Services { get; set; }
        public string HouseType { get; set; } // true => Row , false => Floor
    }
}
