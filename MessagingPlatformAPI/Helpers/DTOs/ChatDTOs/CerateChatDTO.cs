using MessagingPlatformAPI.Helpers.Enums;

namespace MessagingPlatformAPI.Helpers.DTOs.ChatDTOs
{
    public class CerateChatDTO
    {
        public string Name { get; set; }
        public ChatType chatType { get; set; }
        public ICollection<string> MmebersId { get; set; }
    }
}
