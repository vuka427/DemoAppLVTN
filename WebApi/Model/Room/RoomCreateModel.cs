using Domain.Enum;

namespace WebApi.Model.Room
{
    public class RoomCreateModel
    {
        public int BranchId { get; set; }
        public int AreaId { get; set; }
        public int RoomNumber { get; set; }
        public float Acreage { get; set; }
      
        public string IsMezzanine { get; set; }
        public decimal Price { get; set; }
        public int MaxMember { get; set; }
        public ICollection<DeviceCreateModel> Devices { get; set; }

    }
}
