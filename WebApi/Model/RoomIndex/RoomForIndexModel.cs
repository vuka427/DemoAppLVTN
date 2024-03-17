using WebApi.Model.Room;

namespace WebApi.Model.RoomIndex
{
    public class RoomForIndexModel
    {
        public int Id { get; set; }
        public int BranchId { get; set; }
        public int AreaId { get; set; }
        public int RoomNumber { get; set; }
        public bool Status { get; set; }
        public int ElectricNumber { get; set; }
        public int WaterNumber { get; set; }
        public int OldElectricNumber { get; set; }
        public int OldWaterNumber { get; set; }
        public string UpdateElectricDate { get; set; }
        public string UpdateWaterDate { get; set; }
        public string TenantName { get; set; }

        
    }
}
