using MessagingPlatformAPI.Models.UnNeeded;
using System.ComponentModel;

namespace MessagingPlatformAPI.Models
{
    public class PrivateChat
    {
        public Guid Id { get; set; }
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }

        public ICollection<Users_Private> Members { get; set; }
        public ICollection<Message> Messages { get; set; }
        // ChatImage
    }
}
