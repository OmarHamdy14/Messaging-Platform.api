using MessagingPlatformAPI.Models.UnNeeded;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MessagingPlatformAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Bio { get; set; }
        public ICollection<GroupChat> ChatsPrivate { get; set; }
        public ICollection<PrivateChat> ChatsGroup { get; set; }
        public ICollection<StarMessage> StarMessages { get; set; }
        public Guid FinalChatsViewId { get; set; }
        public FinalChatsView FinalChatsView { get; set; }
    }
}
