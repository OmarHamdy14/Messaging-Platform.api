namespace MessagingPlatformAPI.Models.UnNeeded
{
    public class Chat
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsGroup { get; set; }
        public DateTime CreatedDate { get; set; }
        public ICollection<ChatRecord> ChatRecords { get; set; }
        public ICollection<ApplicationUser> Members { get; set; }
        // ChatImage
    }
}
