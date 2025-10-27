using System.ComponentModel.DataAnnotations.Schema;

namespace MessagingPlatformAPI.Models
{
    public class MessageImage
    {
        public Guid Id { get; set; }
        public string PublicId { get; set; }
        public string Url { get; set; }
        public bool IsDeleted { get; set; }

        public Guid MessageId { get; set; }
        [ForeignKey("MessageId")]
        public Message Message { get; set; }
    }
}
