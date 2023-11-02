namespace WebApi.Model.MemberModel
{
    public class MenberForDataTableModel
    {
        public string FullName { get; set; }
        public string DateOfBirth { get; set; }
        public string Cccd { get; set; }
        public string DateOfIssuance { get; set; }
        public string PlaceOfIssuance { get; set; }
        public string PermanentAddress { get; set; }
        public bool IsPermanent { get; set; }
        public string Job { get; set; }
        public string Phone { get; set; }
        public bool Gender { get; set; }
        public bool IsRepresent { get; set; }
        public bool IsActive { get; set; }
        public string CommencingOn { get; set; }
        public string EndingOn { get; set; }

        public string RoomName { get; set; }
        public string BranchName { get; set;}
    }
}
