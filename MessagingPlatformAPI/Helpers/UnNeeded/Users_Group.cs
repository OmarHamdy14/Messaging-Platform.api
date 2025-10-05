namespace MessagingPlatformAPI.Models.UnNeeded
{
    public class Users_Group
    {
        public Guid Id { get; set; }

        public string MemberId { get; set; }
        public ApplicationUser Member { get; set; }

        public Guid GroupChatId { get; set; }
        public GroupChat GroupChat { get; set; }
    }
}
