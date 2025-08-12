using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MessagingPlatformAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<GroupChat> ChatsPrivate { get; set; }
        public ICollection<PrivateChat> ChatsGroup { get; set; }
        public ICollection<StarMessage> StarMessages { get; set; }
    }
}
