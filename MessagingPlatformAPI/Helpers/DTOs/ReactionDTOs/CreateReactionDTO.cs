using MessagingPlatformAPI.Helpers.Enums;
using MessagingPlatformAPI.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace MessagingPlatformAPI.Helpers.DTOs.ReactionDTOs
{
    public class CreateReactionDTO
    {
        public string ReacterId { get; set; }
        public Guid MessageId { get; set; }
        public ReactionType Type { get; set; }
    }
}
