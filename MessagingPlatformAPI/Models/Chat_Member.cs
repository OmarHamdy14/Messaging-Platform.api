namespace MessagingPlatformAPI.Models
{
    public class Chat_Member
    {
        public Guid Id { get; set; }

        public string MemberId { get; set; }
        public ApplicationUser Member { get; set; }

        public bool IsAdmin { get; set; }

        public Guid ChatId { get; set; }
        public Chat Chat { get; set; }
    }
}
