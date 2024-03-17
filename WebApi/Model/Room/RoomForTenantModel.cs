using Domain.Enum;

namespace WebApi.Model.Room
{
    public class RoomForTenantModel
    {
        public int Id { get; set; }
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public int AreaId { get; set; }
        public int RoomNumber { get; set; }
        public float Acreage { get; set; }
        public decimal Price { get; set; }
        public int MaxMember { get; set; }
        public int CurrentMember { get; set; }

        public decimal RentalPrice { get; set; }
        public DateTime CommencingOn { get; set; }
        public DateTime EndingOn { get; set; }
        public ContractStatus Status { get; set; }
        public string BranchAddress { get; set; }
        public HouseType HouseType { get; set; }
        public string AreaName { get; set; }
        public bool IsMezzanine { get; set; }

        public decimal Deposit { get; set; }

        public decimal ElectricityCosts { get; set; }
        public decimal WaterCosts { get; set; }
    }
}
