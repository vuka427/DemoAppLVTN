using Domain.Enum;

namespace WebApi.Model.Room
{
    public class RoomModel
    {
        public int Id { get; set; }
        public int BranchId { get; set; }
        public int AreaId { get; set; }
        public int RoomNumber { get; set; }
        public float Acreage { get; set; }
        public bool IsMezzanine { get; set; }
        public string Status { get; set; }
        public decimal Price { get; set; }
        public int MaxMember { get; set; }

    }
}
