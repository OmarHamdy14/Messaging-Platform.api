using System.ComponentModel;

namespace MessagingPlatformAPI.Models
{
    public class UserSettings
    {
        public Guid Id { get; set; }
        [DefaultValue(false)]
        public bool ReadReceiptsEnabled { get; set; }
        [DefaultValue(false)]
        public bool LastSeenPrivacy { get; set; }
        [DefaultValue(false)]
        public bool IsDoNotDisturbMode { get; set; }
        [DefaultValue(false)]
        public bool IsVerified { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
