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
    }
}
