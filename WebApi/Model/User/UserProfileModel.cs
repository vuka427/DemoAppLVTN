using Domain.Entities;

namespace WebApi.Model.User
{
    public class UserProfileModel
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Ccccd { get; set; }
        public string AvatarUrl { get; set; }

    }
}
