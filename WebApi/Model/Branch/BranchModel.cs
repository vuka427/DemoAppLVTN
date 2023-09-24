using Domain.Enum;

namespace WebApi.Model.Branch
{
    public class BranchModel
    {
        public int Id { get; set; }
        public string BranchName { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public decimal ElectricityCosts { get; set; }
        public decimal WaterCosts { get; set; }
        public decimal InternetCosts { get; set; }
        public decimal GarbageColletionFee { get; set; }
        public string InternalRegulation { get; set; }
        public bool HouseType { get; set; } // true => Row , false => Floor
    }
}
