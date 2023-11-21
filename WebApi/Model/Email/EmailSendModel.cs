using Domain.Entities;
using Domain.Enums;

namespace WebApi.Model.Email
{
    public class EmailSendModel
    {
        public string EmailSender { get; set; }
        public string EmailReceiver { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Status { get; set; }

        public int TenantId { get; set; }
        public int LandlordId { get; set; }
        
    }
}
