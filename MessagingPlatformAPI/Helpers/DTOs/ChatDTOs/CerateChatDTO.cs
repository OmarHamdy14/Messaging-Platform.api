namespace MessagingPlatformAPI.Helpers.DTOs.ChatDTOs
{
    public class CerateChatDTO
    {
        public string Name { get; set; }
        public ICollection<string> MmebersId { get; set; }
    }
}
