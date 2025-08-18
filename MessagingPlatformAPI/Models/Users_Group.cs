namespace MessagingPlatformAPI.Models
{
    public class Users_Group
    {
        public Guid Id { get; set; }

        public Guid GroupChatId { get; set; }
        public GroupChat GroupChat { get; set; }

        public ICollection<ApplicationUser> Users { get; set; }
    }
}
