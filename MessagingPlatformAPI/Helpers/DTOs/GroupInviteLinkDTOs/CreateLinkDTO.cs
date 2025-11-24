namespace MessagingPlatformAPI.Helpers.DTOs.GroupInviteLinkDTOs
{
    public class CreateLinkDTO
    {
        public Guid ChatId { get; set; }
        public string AdminCreatedById { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public int? MaxUses { get; set; }
    }
}
