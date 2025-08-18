namespace MessagingPlatformAPI.Models
{
    public class Users_Private
    {
        public Guid Id { get; set; }

        public Guid PrivateChatId { get; set; }
        public PrivateChat PrivateChat { get; set; }

        public ICollection<ApplicationUser> Users { get; set; }
    }
}
