using MessagingPlatformAPI.Helpers.Enums;

namespace MessagingPlatformAPI.Models
{
    public class MessageStatus
    {
        public Guid Id { get; set; }

        public Guid MessageId { get; set; }
        public Message message { get; set; }

        public string RecieverId { get; set; }
        public ApplicationUser Reciever { get; set; }

        public MessageStatusEnum status { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
