using MessagingPlatformAPI.Models;

namespace MessagingPlatformAPI.Helpers.DTOs.GroupChatDTO
{
    public class CerateGroupChatDTO
    {
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public ICollection<ApplicationUser> Members { get; set; }
        public ICollection<ApplicationUser> Admins { get; set; }
    }
}
