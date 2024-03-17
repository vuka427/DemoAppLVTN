

namespace WebApi.Model.RoomIndex
{
    public class AreaForIndexModel
    {
        public int Id { get; set; }
        public string AreaName { get; set; }
        public ICollection<RoomForIndexModel> Rooms { get; set; } 
    }
}
