using MessagingPlatformAPI.Models.UnNeeded;

namespace MessagingPlatformAPI.Models
{
    public class PrivateChat
    {
        public Guid Id { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<Message> Messages { get; set; }
        // ChatImage
    }
}
