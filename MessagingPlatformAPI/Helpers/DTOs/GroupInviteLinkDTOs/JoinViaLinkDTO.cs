namespace MessagingPlatformAPI.Helpers.DTOs.GroupInviteLinkDTOs
{
    public class JoinViaLinkDTO
    {
        public string token { get; set; }
        public string userId { get; set; }
        public Guid chatId { get; set; }
    }
}
