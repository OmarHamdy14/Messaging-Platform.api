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
    }
}
