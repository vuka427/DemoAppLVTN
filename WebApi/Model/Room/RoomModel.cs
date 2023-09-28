using Domain.Enum;

namespace WebApi.Model.Room
{
    public class RoomModel
    {
        public int Id { get; set; }
        public int RoomNumber { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public float Length { get; set; }
        public bool IsMezzanine { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }
        public int MaxMember { get; set; }

    }
}
