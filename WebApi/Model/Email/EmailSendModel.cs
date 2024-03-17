using Domain.Entities;
using Domain.Enums;

namespace WebApi.Model.Email
{
    public class EmailSendModel
    {
        public int Index { get; set; }
        public string EmailSender { get; set; }
        public string EmailReceiver { get; set; }
        public string ReceiverName { get; set; }
        public string RoomName { get; set; }

        public string Title { get; set; }
        public string Content { get; set; }
        public string Status { get; set; }

        public int TenantId { get; set; }
        public int LandlordId { get; set; }
        public string DateSend { get; set; }
        
    }
}
