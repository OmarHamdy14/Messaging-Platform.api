namespace MessagingPlatformAPI.Models
{
    public class PrivateChat
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid FirstUserId { get; set; }
        public ApplicationUser FirstUser { get; set; }
        public Guid SecondUserId { get; set; }
        public ApplicationUser SecondUser { get; set; }
        public ICollection<ChatRecord> ChatRecords { get; set; }
        // ChatImage
    }
}
