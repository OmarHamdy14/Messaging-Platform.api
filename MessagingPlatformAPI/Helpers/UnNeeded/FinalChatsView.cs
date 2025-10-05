namespace MessagingPlatformAPI.Models.UnNeeded
{
    public class FinalChatsView
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public ICollection<GroupChat> ChatsPrivate { get; set; }
        public ICollection<PrivateChat> ChatsGroup { get; set; }
    }
}
