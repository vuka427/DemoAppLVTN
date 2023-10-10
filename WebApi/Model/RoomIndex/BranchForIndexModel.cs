

namespace WebApi.Model.RoomIndex
{
    public class BranchForIndexModel
    {
        public int Id { get; set; }
        public string BranchName { get; set; }
        public string Address { get; set; }
        public string HouseType { get; set; }

        public ICollection<AreaForIndexModel> Areas { get; set; }
    }
}
