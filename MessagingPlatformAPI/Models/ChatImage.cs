using System.ComponentModel.DataAnnotations.Schema;

namespace MessagingPlatformAPI.Models
{
    public class ChatImage
    {
        public Guid Id { get; set; }
        public string PublicId { get; set; }   // what about puting PublicId&Url in Chat entity ???
        public string Url { get; set; }

        public Guid ChatId { get; set; }
        [ForeignKey("ChatId")]
        public Message Chat { get; set; }
    }
}
