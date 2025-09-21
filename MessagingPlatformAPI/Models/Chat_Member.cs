namespace MessagingPlatformAPI.Models
{
    public class Chat_Member
    {
        public Guid Id { get; set; }

        public string MemberId { get; set; }
        public ApplicationUser Member { get; set; }

        public Guid ChatId { get; set; }
        public Chat GroupChat { get; set; }
    }
}
