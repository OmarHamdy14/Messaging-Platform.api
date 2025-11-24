using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MessagingPlatformAPI.Models
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions op) : base(op)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Chat_Member> Chat_Members { get; set; }
        public DbSet<MessageStatus> MessageStatuses { get; set; }
        public DbSet<UserConnection> UserConnectiones { get; set; }
        public DbSet<Reaction> Reactions { get; set; }
        public DbSet<MessageImage> MessageImages { get; set; }
        public DbSet<ProfileImage> ProfileImages { get; set; }
        public DbSet<ChatImage> ChatImages { get; set; }
        public DbSet<BlockedUser> BlockedUsers { get; set; }
        public DbSet<DeviceToken> deviceTokens { get; set; }
        public DbSet<UserSettings> UserSettings { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<GroupInviteLink> GroupInviteLinks { get; set; }
    }
}
