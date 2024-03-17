namespace WebApi.Model.Member
{
    public class MemberViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Cccd { get; set; }
        public DateTime DateOfIssuance { get; set; }
        public string PlaceOfIssuance { get; set; }
        public string PermanentAddress { get; set; }
        public bool IsPermanent { get; set; }
        public DateTime PermanentDate { get; set; }
        public string Job { get; set; }
        public string Phone { get; set; }
        public bool Gender { get; set; }
        public bool IsRepresent { get; set; }
        public bool IsActive { get; set; }
        public DateTime CommencingOn { get; set; }
        public DateTime EndingOn { get; set; }

        public string RoomName { get; set; }
        public string BranchName { get; set; }

    }
}
