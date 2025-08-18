namespace MessagingPlatformAPI.Models
{
    public class Message
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public Guid? PrivateChatId { get; set; }
        public PrivateChat? PrivateChat { get; set; }

        public Guid? GroupChatId { get; set; }
        public GroupChat? GroupChat { get; set; } // should it be nullable ??
    }
}
