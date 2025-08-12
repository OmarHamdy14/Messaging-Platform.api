namespace MessagingPlatformAPI.Models
{
    public class GroupChat
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public ICollection<ChatRecord> ChatRecords { get; set; }
        public ICollection<ApplicationUser> Members { get; set; }
        public ICollection<ApplicationUser> Admins { get; set; }
        // ChatImage
    }
}
