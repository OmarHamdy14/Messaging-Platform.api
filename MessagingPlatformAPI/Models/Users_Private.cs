namespace MessagingPlatformAPI.Models
{
    public class Users_Private
    {
        public Guid Id { get; set; }

        public string MemberId { get; set; }
        public ApplicationUser Member { get; set; }

        public Guid PrivateChatId { get; set; }
        public PrivateChat PrivateChat { get; set; }
    }
}
