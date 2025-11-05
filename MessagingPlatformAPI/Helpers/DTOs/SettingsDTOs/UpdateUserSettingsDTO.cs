using System.ComponentModel;

namespace MessagingPlatformAPI.Helpers.DTOs.SettingsDTOs
{
    public class UpdateUserSettingsDTO
    {
        [DefaultValue(false)]
        public bool ReadReceiptsEnabled { get; set; }
        [DefaultValue(false)]
        public bool LastSeenPrivacy { get; set; }
        [DefaultValue(false)]
        public bool IsDoNotDisturbMode { get; set; }
        [DefaultValue(false)]
        public bool IsVerified { get; set; }
    }
}
