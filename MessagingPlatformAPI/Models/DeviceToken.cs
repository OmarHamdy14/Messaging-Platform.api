using System.ComponentModel.DataAnnotations.Schema;

namespace MessagingPlatformAPI.Models
{
    public class DeviceToken
    {
        public Guid Id { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        public string Token { get; set; }
        public string DeviceType { get; set; }
        public DateTime CeratedAt { get; set; }

    }
}
