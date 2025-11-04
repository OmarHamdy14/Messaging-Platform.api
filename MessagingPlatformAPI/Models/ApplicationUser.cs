using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace MessagingPlatformAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public bool IsOnline { get; set; }
        public DateTime LastSeen { get; set; }
        
        public bool IsVerified { get; set; }
        public string Bio { get; set; }

        public Guid ProfileImageId { get; set; }
        [ForeignKey("ProfileImageId")]
        public ProfileImage ProfileImage { get; set; }

        public ICollection<Chat_Member> Chats { get; set; }  // ??????

        public ICollection<Message> Messages { get; set; }

        public ICollection<UserConnection> UserConnections { get; set; }

        public ICollection<Reaction> reactions { get; set; }

        public ICollection<BlockedUser> BlockedUsers { get; set; }

        public ICollection<DeviceToken> deviceTokens { get; set; }
        /*public ICollection<Users_Group> ChatsGroup { get; set; }
        public ICollection<Users_Private> ChatsPrivate { get; set; }*/
    }
}
