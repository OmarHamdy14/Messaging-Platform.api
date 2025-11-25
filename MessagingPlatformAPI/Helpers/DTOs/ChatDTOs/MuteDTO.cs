namespace MessagingPlatformAPI.Helpers.DTOs.ChatDTOs
{
    public class MuteDTO
    {
        public string UserId { get; set; }
        public Guid ChatId { get; set; }
        public TimeSpan duration { get; set; }
    }
}
