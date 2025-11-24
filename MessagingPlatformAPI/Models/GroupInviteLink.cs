namespace MessagingPlatformAPI.Models
{
    public class GroupInviteLink
    {
        public string Token { get; set; }
        public Guid ChatId { get; set; }
        public string AdminCreatedById { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public int? MaxUses { get; set; }
        public int UsesCount { get; set; } = 0;
        public bool IsRevoked { get; set; } = false;
    }
}
