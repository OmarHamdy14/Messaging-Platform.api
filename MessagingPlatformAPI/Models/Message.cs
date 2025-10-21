using System.ComponentModel;

namespace MessagingPlatformAPI.Models
{
    public class Message
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }

        public DateTime? DeletedAt { get; set; } 
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }

        public DateTime? EditedAt { get; set; }
        [DefaultValue(false)]
        public bool IsEdited { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public Guid ChatId { get; set; }
        public Chat Chat { get; set; } 

        public ICollection<MessageStatus> messageStatuses { get; set; }

        public ICollection<Reaction> reactions { get; set; }


        /*
        public Guid? PrivateChatId { get; set; }
        public PrivateChat PrivateChat { get; set; } // should it be nullable ??

        public Guid? GroupChatId { get; set; }
        public GroupChat GroupChat { get; set; } // should it be nullable ??
        */
    }
}
