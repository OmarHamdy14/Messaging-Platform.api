namespace MessagingPlatformAPI.Models
{
    public class Chat
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }

        public ICollection<Chat_Member> Members { get; set; }
        public ICollection<Message> Messages { get; set; }
        // ChatImage
    }
}
