namespace MessagingPlatformAPI.Helpers.DTOs.MessageDTOs
{
    public class CreateMessageDTO
    {
        public string Content { get; set; }
        public string UserId { get; set; }
        public Guid ChatId { get; set; }

        public bool IsForward { get; set; }
        /*public Guid? PrivateChatId { get; set; }
        public Guid GroupChatId { get; set; }*/
    }
}
