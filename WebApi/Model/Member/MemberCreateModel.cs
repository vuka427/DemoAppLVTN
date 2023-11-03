namespace WebApi.Model.Member
{
    public class MemberCreateModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Cccd { get; set; }
        public DateTime DateOfIssuance { get; set; }
        public string PlaceOfIssuance { get; set; }
        public string PermanentAddress { get; set; }
        public string IsPermanent { get; set; }
        public DateTime PermanentDate { get; set; }
        public string Job { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; }
        public DateTime CommencingOn { get; set; }
        public DateTime? EndingOn { get; set; }

     
    }
}
