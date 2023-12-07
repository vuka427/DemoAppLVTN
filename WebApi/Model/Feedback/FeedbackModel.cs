using Domain.Enum;

namespace WebApi.Model.Feedback
{
    public class FeedbackModel
    {
        public int Index { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Status { get; set; }

        public string ReceiverName { get; set; }
        public string RoomName { get; set; }

        public string BranchName { get; set; }

        public string CreatedDate { get; set; }
    }
}
