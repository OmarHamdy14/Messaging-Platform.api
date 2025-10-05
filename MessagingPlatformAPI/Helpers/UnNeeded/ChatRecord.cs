namespace MessagingPlatformAPI.Models.UnNeeded
{
    public class ChatRecord
    {
        public Guid Id { get; set; }
        public string MessageContent { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsSeen { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsStar { get; set; }
        public string SenderId { get; set; }
        public ApplicationUser Sender { get; set; }
        public Guid ChatId { get; set; }
        public Chat Chat { get; set; }
        //public ICollection<StarMessage> StarMessageS { get; set; } // ?
        // seen by who (like in groups)
    }
}
