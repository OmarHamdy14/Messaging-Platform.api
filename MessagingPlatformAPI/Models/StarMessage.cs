namespace MessagingPlatformAPI.Models
{
    public class StarMessage
    {
        public Guid Id { get; set; }
        //public DateTime CreatedDate { get; set; }
        public Guid ChatRecordId { get; set; }
        public ChatRecord ChatRecord { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
