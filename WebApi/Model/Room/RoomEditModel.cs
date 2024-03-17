namespace WebApi.Model.Room
{
    public class RoomEditModel
    {
        public int Id { get; set; }
        public int RoomNumber { get; set; }
        public float Acreage { get; set; }
        public string IsMezzanine { get; set; }
        public decimal Price { get; set; }
        public int MaxMember { get; set; }
        public ICollection<DeviceEditModel> Devices { get; set; }

    }
}
