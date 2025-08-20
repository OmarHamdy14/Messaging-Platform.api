using MessagingPlatformAPI.Models.UnNeeded;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MessagingPlatformAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Bio { get; set; }
        public ICollection<Users_Group> ChatsGroup { get; set; }
        public ICollection<Users_Private> ChatsPrivate { get; set; }
        public ICollection<Message> Messages { get; set; }
    }
}
