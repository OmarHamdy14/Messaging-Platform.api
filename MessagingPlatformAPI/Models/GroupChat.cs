using MessagingPlatformAPI.Models.UnNeeded;

namespace MessagingPlatformAPI.Models
{
    public class GroupChat
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<Message> Messages { get; set; }
        // ChatImage
    }
}
