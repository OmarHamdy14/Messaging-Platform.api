using System.ComponentModel.DataAnnotations.Schema;

namespace MessagingPlatformAPI.Models
{
    public class Chat
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }

        public bool IsMuted { get; set; }

        public string? ChatCreatorId { get; set; }
        [ForeignKey("ChatCreatorId")]
        public ApplicationUser ChatCreator { get; set; }

        public Guid PinnedMessageId { get; set; }
        public Message PinnedMessage { get; set; }

        public ICollection<Chat_Member> Members { get; set; }

        public ICollection<Message> Messages { get; set; }
        // ChatImage
    }
}
