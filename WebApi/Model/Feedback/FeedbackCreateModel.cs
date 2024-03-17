namespace WebApi.Model.Feedback
{
    public class FeedbackCreateModel
    { 
        public int contractId { get; set; }
        public string title { get; set; }
        public string content { get; set; }
    }
}
