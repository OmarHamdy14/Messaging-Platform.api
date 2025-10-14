using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MessagingPlatformAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public DateTime LastSeen { get; set; }
        
        public string Bio { get; set; }
        public ICollection<Chat_Member> Chats { get; set; }  // ??????
        public ICollection<Message> Messages { get; set; }
        /*public ICollection<Users_Group> ChatsGroup { get; set; }
        public ICollection<Users_Private> ChatsPrivate { get; set; }*/
    }
}
