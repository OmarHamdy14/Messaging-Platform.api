using MessagingPlatformAPI.Helpers.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace MessagingPlatformAPI.Models
{
    public class Reaction
    {
        public Guid Id { get; set; }

        public string ReacterId { get; set; }
        [ForeignKey("ReacterId")]
        public ApplicationUser Reacter { get; set; }

        public Guid MessageId { get; set; }
        [ForeignKey("MessageId")]
        public Message Message { get; set; }

        public ReactionType Type { get; set; }
    }
}
