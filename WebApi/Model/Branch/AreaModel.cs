using WebApi.Model.Room;

namespace WebApi.Model.Branch
{
    public class AreaModel
    {
        public int Id { get; set; }
        public string AreaName { get; set; }
        public string Description { get; set; }
        public ICollection<RoomModel> Rooms { get; set; } 
    }
}
