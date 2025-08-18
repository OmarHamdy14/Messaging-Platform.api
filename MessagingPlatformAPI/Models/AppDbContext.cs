using MessagingPlatformAPI.Models.UnNeeded;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MessagingPlatformAPI.Models
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions op) : base(op)
        {

        }
        public DbSet<ApplicationUser> Users { get; set; }
    }
}
