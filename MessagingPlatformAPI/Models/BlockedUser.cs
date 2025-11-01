using System.ComponentModel.DataAnnotations.Schema;

namespace MessagingPlatformAPI.Models
{
    public class BlockedUser
    {
        public Guid Id { get; set; }

        public string BlockerId { get; set; }
        [ForeignKey("BlockerId")]
        public ApplicationUser Blocker { get; set; }

        public string BlockedId { get; set; }
        [ForeignKey("BlockedId")]
        public ApplicationUser Blocked { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
